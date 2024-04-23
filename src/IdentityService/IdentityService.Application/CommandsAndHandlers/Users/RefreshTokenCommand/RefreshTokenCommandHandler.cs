using AutoMapper;
using IdentityService.Application.DomainServices.EmailServices;
using IdentityService.Application.ViewModels.ApplicationUsers.Response;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RefreshTokenEntity;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using SharedKernel.DomainContracts;
using SharedKernel.DomainImplementations.DomainErrors;
using SharedKernel.Helpers.Configuration;
using SharedKernel.Helpers.Epoch;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace IdentityService.Application.CommandsAndHandlers.Users.RefreshTokenCommand;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthenticateResponse>
{
    public RefreshTokenCommandHandler(
        IMyUnitOfWork unitOfWork,
        ITrackableRepository<User> userRepository,
        ITrackableRepository<Role> roleRepository,
        ITrackableRepository<RefreshToken> refreshTokenRepository,
        IMapper mapper,
        IOptions<MyConfigurationValues> appSettings,
        IEmailService emailService,
        IOptions<JwtIssuerOptions> jwtOptions

    )
    {
        UnitOfWork = unitOfWork;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        RefreshTokenRepository = refreshTokenRepository;
        Mapper = mapper;
        AppSettings = appSettings.Value;
        EmailService = emailService;
        JWTOptions = jwtOptions.Value;
    }

    private IMyUnitOfWork UnitOfWork { get; }
    private ITrackableRepository<Role> RoleRepository { get; }
    public ITrackableRepository<RefreshToken> RefreshTokenRepository { get; }
    private ITrackableRepository<User> UserRepository { get; }

    private IMapper Mapper { get; }
    private MyConfigurationValues AppSettings { get; }
    private IEmailService EmailService { get; }
    public JwtIssuerOptions JWTOptions { get; }

    private async Task<(RefreshToken, User)> GetRefreshTokenAsync(string token)
    {
        if (string.IsNullOrEmpty(token)) throw new ArgumentNullException("EmailVerificationToken invalid");
        var applicationUser = await UserRepository.Queryable()
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
    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return RefreshToken.NewRefreshTokenDraft(RandomTokenString(), ipAddress);
    }
    public async Task<AuthenticateResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var returnValue = new AuthenticateResponse
        {
            Success = false,
            Message = ""
        };

        if (string.IsNullOrEmpty(command.token) || string.IsNullOrEmpty(command.ipAddress))
            throw new ArgumentNullException("EmailVerificationToken and/or ip address are invalid");

        var (refreshToken, applicationUser) = await GetRefreshTokenAsync(command.token);

        try
        {
            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(command.ipAddress);
            var theRefreshTokenDraft = RefreshToken.NewRefreshTokenDraft(newRefreshToken.Token, command.ipAddress);
            applicationUser.AddRefreshToken(theRefreshTokenDraft);
            applicationUser.RemoveStaleRefreshTokens();

            UserRepository.Update(applicationUser);
            var saveResult = await UnitOfWork.SaveChangesAsync();

            // generate new jwt
            var jwtToken = GenerateJwtTokenAsync(applicationUser);

            var response = Mapper.Map<AuthenticateResponse>(applicationUser);

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

    private async Task<string> GenerateJwtTokenAsync(User applicationUser)
    {
        if (applicationUser == null) throw new ArgumentNullException("UpdateApplicationUserRequest invalid");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
        var jti = await JWTOptions.JtiGenerator();
        var issuedAt = JWTOptions.IssuedAt;
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
}