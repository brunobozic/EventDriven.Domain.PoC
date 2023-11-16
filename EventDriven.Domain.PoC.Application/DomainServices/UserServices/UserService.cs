using AutoMapper;
using EventDriven.Domain.PoC.Application.DomainServices.EmailServices;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RefreshToken;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.DomainErrors;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Epoch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;
using BC = BCrypt.Net.BCrypt;

namespace EventDriven.Domain.PoC.Application.DomainServices.UserServices
{
    public class UserService : Service<User>, IUserService
    {
        #region ctor

        public UserService(
            IMyUnitOfWork unitOfWork,
            ITrackableRepository<User> repository,
            ITrackableRepository<Role> roleRepository,
            IMapper mapper,
            IOptions<JwtIssuerOptions> jwtOptions,
            IOptions<MyConfigurationValues> appSettings,
            IEmailService emailService) : base(repository)
        {
            UnitOfWork = unitOfWork;
            RoleRepository = roleRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
            _jwtOptions = jwtOptions.Value;
        }

        #endregion ctor

        #region public props

        public ITrackableRepository<Role> RoleRepository { get; }

        #endregion public props

        #region private props

        private readonly MyConfigurationValues _appSettings;
        private readonly IOptions<MyConfigurationValues> _appSettingsSnap;
        private readonly IEmailService _emailService;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IMapper _mapper;
        private IMyUnitOfWork UnitOfWork { get; }

        #endregion private props

        #region public methods

        public async Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model, string ipAddress)
        {
            if (model == null || string.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException("[AuthenticateRequest] is null, and/or ip address are invalid");
            if (string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException("[AuthenticateRequest] => [Password] invalid");
            if (string.IsNullOrEmpty(model.Email))
                throw new ArgumentNullException("[AuthenticateRequest] => [Email] invalid");

            var returnValue = new AuthenticateResponse
            {
                Success = false,
                Message = ""
            };

            var applicationUser = await Repository.Queryable().SingleOrDefaultAsync(x => x.Email.ToUpper().Trim() == model.Email.ToUpper().Trim());

            if (applicationUser == null ||
                applicationUser.GetCurrentRegistrationStatus() != RegistrationStatusEnum.WaitingForVerification ||
                !BC.Verify(model.Password, applicationUser.PasswordHash))
                throw new AppException("Email or password is incorrect, or the user has not verified his account [ " + model.Email.ToUpper().Trim() + " ]");

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = await GenerateJwtTokenAsync(applicationUser);
            var refreshToken = GenerateRefreshToken(ipAddress);

            try
            {
                // add a newly created refresh token to the data store
                applicationUser.AddRefreshToken(refreshToken);
                // remove old refresh tokens from the data store
                applicationUser.RemoveStaleRefreshTokens();

                // persist changes throught the aggregate root
                Repository.Update(applicationUser);

                // fire the unit of work transaction
                var saveResult = await UnitOfWork.SaveChangesAsync();

                // map the service layer response into a view model
                var response = _mapper.Map<AuthenticateResponse>(applicationUser);

                response.JwtToken = jwtToken;
                response.RefreshToken = refreshToken.Token;
                response.Success = true;
                response.Message = "Authenticated";

                // return the created jwt token and the refresh token so they can be used by the front end
                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Problem authenticating user with email: [ " + applicationUser.Email + " ]", ex);

                returnValue.Message = ex.Message;
                returnValue.InnerMessage = ex.InnerException?.Message;
                returnValue.UserFriendlyMessage = "Problem logging in, generating a jwt token or generating a refresh token.";

                return returnValue;
            }
        }

        public async Task<ApplicationUserResponse> GetAllAsync()
        {
            var applicationUsers = await Repository.Queryable().ToListAsync();

            var retVal = new ApplicationUserResponse
            {
                Success = true,
                ViewModels = _mapper.Map<List<UserViewModel>>(applicationUsers)
            };

            return retVal;
        }


        public async Task<ApplicationUserResponse> GetByIdAsync(Guid id)
        {
            var applicationUser = await Repository.Queryable().Where(user => user.Id == id).SingleOrDefaultAsync();

            if (applicationUser == null) throw new KeyNotFoundException(id.ToString());

            var retVal = new ApplicationUserResponse
            {
                Success = true,
                ViewModel = _mapper.Map<UserViewModel>(applicationUser)
            };

            return retVal;
        }

        public async Task<AuthenticateResponse> RefreshTheTokenAsync(string token, string ipAddress)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException("EmailVerificationToken and/or ip address are invalid");

            var (refreshToken, applicationUser) = await GetRefreshTokenAsync(token);

            var returnValue = new AuthenticateResponse
            {
                Success = false,
                Message = ""
            };

            try
            {
                // replace old refresh token with a new one and save
                var newRefreshToken = GenerateRefreshToken(ipAddress);
                var theRefreshTokenDraft = RefreshToken.NewRefreshTokenDraft(newRefreshToken.Token, ipAddress);
                applicationUser.AddRefreshToken(theRefreshTokenDraft);
                applicationUser.RemoveStaleRefreshTokens();

                Repository.Update(applicationUser);
                var saveResult = await UnitOfWork.SaveChangesAsync();

                // generate new jwt
                var jwtToken = GenerateJwtTokenAsync(applicationUser);

                var response = _mapper.Map<AuthenticateResponse>(applicationUser);

                response.JwtToken = await jwtToken;
                response.RefreshToken = newRefreshToken.Token;
                response.Success = true;
                response.Message = "EmailVerificationToken refreshed";

                return response;
            }
            catch (Exception ex)
            {
                Log.Error("Problem refreshing token for user with email: [ " + applicationUser.Email + " ]", ex);

                returnValue.Success = false;
                returnValue.Message = ex.Message;
                returnValue.InnerMessage = ex.InnerException?.Message;
                returnValue.UserFriendlyMessage = "Problem refreshing token";

                return returnValue;
            }
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterUserRequest model, string origin)
        {
            if (model == null || string.IsNullOrEmpty(origin))
                throw new ArgumentNullException("RegisterApplicationUserRequest and/or origin are invalid");
            if (string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException("RegisterApplicationUserRequest Password invalid");
            if (string.IsNullOrEmpty(model.ConfirmPassword))
                throw new ArgumentNullException("RegisterApplicationUserRequest ConfirmPassword invalid");
            if (string.IsNullOrEmpty(model.Email))
                throw new ArgumentNullException("RegisterApplicationUserRequest Email invalid");
            if (string.IsNullOrEmpty(model.FirstName))
                throw new ArgumentNullException("RegisterApplicationUserRequest FirstName invalid");
            if (string.IsNullOrEmpty(model.LastName))
                throw new ArgumentNullException("RegisterApplicationUserRequest LastName invalid");

            var returnValue = new RegisterResponse
            {
                Success = false,
                Message = ""
            };

            var applicationUser = _mapper.Map<User>(model);

            // validate
            if (await Repository.Queryable().AnyAsync(x => x.Email == model.Email))
            {
                // TODO: this should be part of event driven sequence
                // TODO: dont think this will be fired automatically as new user will not be inserted hence no domain event will be attached
                // send already registered error in email to prevent applicationUser enumeration
                applicationUser.SendAlreadyRegisteredEmail(origin);

                returnValue.Message = "Operation failed";

                return returnValue;
            }

            var defaultRole = await RoleRepository.Queryable().Where(r => r.Name.Trim().ToUpper() == "GUEST")
                .SingleOrDefaultAsync();
            var roleAssigner = await Repository.Queryable()
                .Where(u => u.Id == Guid.Parse("2da4d020-5ac7-453b-a28a-e621aeb9c109")).SingleOrDefaultAsync();
            applicationUser.AddRole(defaultRole, roleAssigner);
            applicationUser.AddVerificationToken(RandomTokenString());
            applicationUser.AddPasswordHash(model.Password);

            try
            {
                Repository.Insert(applicationUser);
                var saveResult = await UnitOfWork.SaveChangesAsync();

                returnValue.Success = true;
                returnValue.Message = "Verification mail queued up for sending";
            }
            catch (Exception ex)
            {
                Log.Error("Problem registering user with email: [ " + applicationUser.Email + " ]", ex);

                returnValue.Success = false;
                returnValue.Message = ex.Message;
                returnValue.InnerMessage = ex.InnerException?.Message;
                returnValue.UserFriendlyMessage = "Problem registering user";
            }

            return returnValue;
        }

        public async Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException("EmailVerificationToken and/or ip address are invalid");

            var (refreshToken, applicationUser) = await GetRefreshTokenAsync(token);

            var returnValue = new RevokeTokenResponse
            {
                Success = false,
                Message = ""
            };

            try
            {
                // revoke token and save
                var revoker = await Repository.Queryable()
                   .Where(u => u.Id == Guid.Parse(ApplicationWideConstants.SYSTEM_USER)).SingleOrDefaultAsync();
                applicationUser.RevokeToken(token, ipAddress, revoker);
                Repository.Update(applicationUser);
                var saveResult = await UnitOfWork.SaveChangesAsync();

                returnValue.Success = true;
                returnValue.Message = "EmailVerificationToken revoked";
            }
            catch (Exception ex)
            {
                Log.Error("Problem revoking token for user with email: [ " + applicationUser.Email + " ]", ex);

                returnValue.Success = false;
                returnValue.Message = ex.Message;
                returnValue.InnerMessage = ex.InnerException?.Message;
                returnValue.UserFriendlyMessage = "Problem revoking token";
            }

            return returnValue;
        }

        public async Task<ValidateResetTokenResponse> ValidateResetTokenAsync(ValidateResetTokenRequest model)
        {
            if (model == null) throw new ArgumentNullException("ValidateResetTokenRequest invalid");
            if (string.IsNullOrEmpty(model.Token))
                throw new ArgumentNullException("ValidateResetTokenRequest token invalid");

            var returnValue = new ValidateResetTokenResponse
            {
                Success = false,
                Message = ""
            };

            var applicationUser = await Repository.Queryable().SingleOrDefaultAsync(x =>
                x.ResetToken == model.Token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (applicationUser == null)
            {
                returnValue.Message = "Not found";
                returnValue.InnerMessage = "";
                returnValue.UserFriendlyMessage = "Not found";
            }
            else
            {
                returnValue.Success = true;
            }

            return returnValue;
        }

        public async Task<VerifyEmailResponse> VerifyEmailAsync(string token, string origin)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException("EmailVerificationToken invalid");

            var returnValue = new VerifyEmailResponse
            {
                Success = false,
                Message = ""
            };

            var applicationUser =
                await Repository.Queryable().SingleOrDefaultAsync(x => x.EmailVerificationToken == token);

            if (applicationUser == null) throw new AppException("Verification failed");

            try
            {
                applicationUser.SetEmailIsVerified();
                Repository.Update(applicationUser);
                var saveResult = await UnitOfWork.SaveChangesAsync();

                returnValue.Success = true;
                returnValue.Message = "Verification successful";
            }
            catch (Exception ex)
            {
                Log.Error("Problem verifying email: [ " + applicationUser.Email + " ]", ex);

                returnValue.Success = false;
                returnValue.Message = ex.Message;
                returnValue.InnerMessage = ex.InnerException?.Message;
                returnValue.UserFriendlyMessage = "Problem verifying email";
            }

            return returnValue;
        }

        //public async Task<ForgotPasswordResponse> ForgotPasswordAsync(InitiateForgotPasswordRequest model, string origin)
        //{
        //    if (model == null || string.IsNullOrEmpty(origin))
        //        throw new ArgumentNullException("InitiateForgotPasswordRequest and/or origin are invalid");
        //    if (string.IsNullOrEmpty(model.Email))
        //        throw new ArgumentNullException("InitiateForgotPasswordRequest Email invalid");

        //    var returnValue = new ForgotPasswordResponse
        //    {
        //        Success = false,
        //        Message = ""
        //    };

        //    var applicationUser = await Repository.Queryable().SingleOrDefaultAsync(x => x.Email == model.Email);

        //    // always return ok response to prevent email enumeration
        //    if (applicationUser == null)
        //    {
        //        returnValue.Message = "The requested user does not exist, unable to change password.";
        //        returnValue.InnerMessage = "";
        //        returnValue.UserFriendlyMessage = "The requested user does not exist, unable to change password.";

        //        return returnValue;
        //    }

        //    try
        //    {
        //        // create reset token that expires after 1 day
        //        applicationUser.CreatePasswordResetToken(RandomTokenString());
        //        Repository.Update(applicationUser);
        //        var saveResult = await UnitOfWork.SaveChangesAsync();

        //        // send email
        //        SendPasswordResetEmail(applicationUser, origin);

        //        returnValue.Success = true;
        //        returnValue.Message = "Forgotten password recovery success";
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("Problem recovering password for user with email: [ " + applicationUser.Email + " ]", ex);

        //        returnValue.Success = false;
        //        returnValue.Message = ex.Message;
        //        returnValue.InnerMessage = ex.InnerException?.Message;
        //        returnValue.UserFriendlyMessage = "Problem recovering password";
        //    }

        //    return returnValue;
        //}
        //public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest model)
        //{
        //    if (model == null) throw new ArgumentNullException("ResetPasswordRequest invalid");
        //    if (string.IsNullOrEmpty(model.Token))
        //        throw new ArgumentNullException("ResetPasswordRequest token invalid");
        //    if (string.IsNullOrEmpty(model.Password))
        //        throw new ArgumentNullException("ResetPasswordRequest password invalid");

        //    var returnValue = new ResetPasswordResponse
        //    {
        //        Success = false,
        //        Message = ""
        //    };

        //    var applicationUser = await Repository.Queryable().SingleOrDefaultAsync(x =>
        //        x.ResetToken == model.Token &&
        //        x.ResetTokenExpires > DateTime.UtcNow);

        //    if (applicationUser == null)
        //        throw new AppException("Invalid token");

        //    try
        //    {
        //        // update password and remove reset token
        //        applicationUser.UpdatePasswordRemoveResetToken(model.Password);
        //        Repository.Update(applicationUser);
        //        var saveResult = await UnitOfWork.SaveChangesAsync();

        //        returnValue.Success = true;
        //        returnValue.Message = "Password reset.";
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("Problem resetting password for user with email: [ " + applicationUser.Email + " ]", ex);
        //        returnValue.Message = ex.Message;
        //        returnValue.InnerMessage = ex.InnerException?.Message;
        //        returnValue.UserFriendlyMessage = "Problem resetting password";
        //    }

        #region CUD

        public async Task<CreateApplicationUserResponse> CreateAsync(CreateApplicationUserRequest model,
            User currentlyLoggedUser)
        {
            if (model == null) throw new ArgumentNullException("CreateApplicationUserCommand invalid");

            var returnValue = new CreateApplicationUserResponse
            {
                Success = false,
                Message = ""
            };

            // validate
            if (await Repository.Queryable().AnyAsync(x => x.Email == model.Email))
                throw new AppException($"Email '{model.Email}' is already registered");
            var creator = await Repository.Queryable()
                .Where(u => u.Id == Guid.Parse(ApplicationWideConstants.SYSTEM_USER)).SingleOrDefaultAsync();
            // map model to new applicationUser object
            var newApplicationUser = User.NewDraft(
                model.Id
                , model.Email
                , model.Email
                , model.FirstName
                , model.LastName
                , model.Role
                , "Guest"
                , creator
                , model.Origin
            );

            try
            {
                var defaultRole = await RoleRepository.Queryable().Where(r => r.Name.Trim().ToUpper() == "GUEST")
                    .SingleOrDefaultAsync();
                var roleAssigner = await Repository.Queryable()
                    .Where(u => u.Id == Guid.Parse(ApplicationWideConstants.SYSTEM_USER)).SingleOrDefaultAsync();
                newApplicationUser.AddRole(defaultRole, roleAssigner);
                Repository.Insert(newApplicationUser);
                var saveResult = await UnitOfWork.SaveChangesAsync();

                var appUserToReturn = await Repository.Queryable()
                    .Where(user => user.UserName == newApplicationUser.UserName).SingleAsync();
                returnValue.Success = true;
                returnValue.ViewModel = _mapper.Map<UserViewModel>(appUserToReturn);
                returnValue.Message = "Item created.";
            }
            catch (Exception ex)
            {
                Log.Error("Problem creating user with email: [ " + newApplicationUser.Email + " ]", ex);

                returnValue.Success = false;
                returnValue.Message = ex.Message;
                returnValue.InnerMessage = ex.InnerException?.Message;
                returnValue.UserFriendlyMessage =
                    "Problem creating user with email: [ " + newApplicationUser.Email + " ]";
            }

            return returnValue;
        }

        public async Task<DeleteUserResponse> DeleteAsync(Guid id, User currentlyLoggedUser)
        {
            if (id == Guid.Empty) throw new ArgumentNullException("Delete invalid");

            var applicationUser = await Repository.Queryable().Where(user => user.Id == id).SingleAsync();

            var returnValue = new DeleteUserResponse
            {
                Success = false,
                Message = ""
            };

            try
            {
                await Repository.DeleteAsync(applicationUser);
                var saveResult = await UnitOfWork.SaveChangesAsync();

                returnValue.Success = true;
                returnValue.ViewModel = _mapper.Map<UserViewModel>(applicationUser);
                returnValue.Message = "Item deleted.";
            }
            catch (Exception ex)
            {
                Log.Error("Problem deleting user with email: [ " + applicationUser.Email + " ]", ex);

                returnValue.Message = ex.Message;
                returnValue.InnerMessage = ex.InnerException?.Message;
            }

            return returnValue;
        }

        public async Task<UpdateRequestResponse> UpdateAsync(Guid id, UpdateApplicationUserRequest model,
                    User currentlyLoggedUser)
        {
            if (model == null) throw new ArgumentNullException("UpdateApplicationUserRequest invalid");
            if (string.IsNullOrEmpty(model.Email))
                throw new ArgumentNullException("UpdateApplicationUserRequest email invalid");
            if (string.IsNullOrEmpty(model.Password))
                throw new ArgumentNullException("UpdateApplicationUserRequest password invalid");

            var returnValue = new UpdateRequestResponse
            {
                Success = false,
                Message = ""
            };

            var applicationUser = await Repository.Queryable().Where(user => user.Id == id).SingleAsync();

            // validate
            if (applicationUser.Email != model.Email &&
                await Repository.Queryable().AnyAsync(x => x.Email == model.Email))
                throw new AppException($"Email '{model.Email}' is already taken");

            //// hash password if it was entered
            //if (!string.IsNullOrEmpty(model.Password))
            //    applicationUser.PasswordHash = BC.HashPassword(model.Password);

            // copy model to applicationUser and save
            _mapper.Map(model, applicationUser);

            try
            {
                Repository.Update(applicationUser);
                var saveResult = await UnitOfWork.SaveChangesAsync();
                var appUserToReturn = await Repository.Queryable()
                    .Where(user => user.UserName == applicationUser.UserName).SingleAsync();
                returnValue.Success = true;
                returnValue.ViewModel = _mapper.Map<UserViewModel>(appUserToReturn);
                returnValue.Message = "Item updated.";
            }
            catch (Exception ex)
            {
                Log.Error("Problem updating user with email: [ " + applicationUser.Email + " ]", ex);

                returnValue.Success = false;
                returnValue.Message = ex.Message;
                returnValue.InnerMessage = ex.InnerException?.Message;
                returnValue.UserFriendlyMessage = "Problem updating user with email:[ " + applicationUser.Email + " ]";
            }

            return returnValue;
        }

        #endregion CUD

        #endregion public methods

        #region private methods

        private async Task<string> GenerateJwtTokenAsync(User applicationUser)
        {
            if (applicationUser == null) throw new ArgumentNullException("UpdateApplicationUserRequest invalid");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var jti = await _jwtOptions.JtiGenerator();
            var issuedAt = _jwtOptions.IssuedAt;
            var issuedAtString = UnixEpoch.ToUnixEpochDate(issuedAt).ToString();
            var fullName = applicationUser.FirstName + " " + applicationUser.LastName;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(15),
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", applicationUser.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, jti),
                    new Claim(JwtRegisteredClaimNames.Iat, issuedAtString, ClaimValueTypes.Integer64),
                    new Claim(JwtClaimNameConstants.ID_CLAIM_NAME, applicationUser.Id.ToString()),
                    //new Claim(JwtClaimNameConstants.ROLE_ID_CLAIM_NAME, applicationUser.Role.ToString()),
                    new Claim(JwtRegisteredClaimNamesCustom.APPLICATION_TYPE, "DockerPoC"),
                    new Claim(JwtRegisteredClaimNamesCustom.FULL_NAME, fullName),
                    new Claim(JwtRegisteredClaimNamesCustom.USERNAME, applicationUser.UserName),
                    new Claim(JwtRegisteredClaimNamesCustom.EMAIL, applicationUser.Email),
                    new Claim(JwtRegisteredClaimNamesCustom.USER_ID, applicationUser.Id.ToString()),
                    new Claim(JwtRegisteredClaimNamesCustom.ORGANIZATION_UNITS, ""),
                    //new Claim(JwtRegisteredClaimNamesCustom.ROLES, userRoles.ToString()),
                    new Claim(JwtClaimNameConstants.Organization_UNIT, "")
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return RefreshToken.NewRefreshTokenDraft(RandomTokenString(), ipAddress);
        }

        private async Task<(RefreshToken, User)> GetRefreshTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) throw new ArgumentNullException("EmailVerificationToken invalid");
            var applicationUser = await Repository.Queryable()
                .SingleOrDefaultAsync(u => u.Active && u.RefreshTokens.Any(t => t.Token == token));
            if (applicationUser == null) throw new AppException("Invalid token");
            var refreshToken = applicationUser.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new AppException("Invalid token");

            return (refreshToken, applicationUser);
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        #endregion private methods
    }
}