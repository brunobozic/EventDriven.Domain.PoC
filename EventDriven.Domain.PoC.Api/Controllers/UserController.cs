using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventDriven.Domain.PoC.Api.Rest.Attributes;
using EventDriven.Domain.PoC.Api.Rest.Controllers.BaseControllerType;
using EventDriven.Domain.PoC.Api.Rest.Controllers.Contracts;
using EventDriven.Domain.PoC.Application;
using EventDriven.Domain.PoC.Application.CommandHandlers.Addresses;
using EventDriven.Domain.PoC.Application.CommandHandlers.Roles;
using EventDriven.Domain.PoC.Application.CommandHandlers.Users.CUD;
using EventDriven.Domain.PoC.Application.DomainServices.UserServices;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork;
using EventDriven.Domain.PoC.SharedKernel.Helpers;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenTracing;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("user")]
    public class UserController : BaseController, IUserController
    {
        #region ctor

        public UserController(
            IUserService applicationUserService,
            IMyUnitOfWork unitOfWork
            , IOptionsSnapshot<MyConfigurationValues> configurationValues
            , IMapper mapper
            , IMediator mediator
            , IMemoryCache memCache
            , IHttpContextAccessor contextAccessor
            , ITracer tracer
        ) : base(unitOfWork, mapper, configurationValues, memCache, contextAccessor, applicationUserService)
        {
            _applicationUserService = applicationUserService;
            _configurationValues = configurationValues.Value;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _mediator = mediator;
            _tracer = tracer;
        }

        #endregion ctor

        [HttpPost("authenticate")]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<AuthenticateResponse>> AuthenticateAsync(AuthenticateRequest model,
            CancellationToken ct)
        {
            var serviceLayerResponse = await _applicationUserService.AuthenticateAsync(model, IpAddress());

            if (serviceLayerResponse.Success)
            {
                SetTokenCookie(serviceLayerResponse.RefreshToken);

                if (_contextAccessor.HttpContext != null)
                    _contextAccessor.HttpContext.Items["ApplicationUser"] = serviceLayerResponse;

                return Ok(serviceLayerResponse);
            }

            return BadRequest(serviceLayerResponse.Message);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<AuthenticateResponse>> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new {message = "Refresh Token (Request cookie) is required"});

            var serviceLayerResponse = await _applicationUserService.RefreshTheTokenAsync(refreshToken, IpAddress());

            if (serviceLayerResponse.Success)
            {
                SetTokenCookie(serviceLayerResponse.RefreshToken);

                return Ok(serviceLayerResponse.Message);
            }

            return BadRequest(serviceLayerResponse.Message);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<RevokeTokenResponse>> RevokeTokenAsync(RevokeTokenRequest model,
            CancellationToken ct)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required");

            //// users can revoke their own tokens and admins can revoke any tokens
            //if (!applicationUser.OwnsToken(token) && applicationUser.Role != Role.Admin)
            //    return Unauthorized("Unauthorized");

            var serviceLayerResponse = await _applicationUserService.RevokeTokenAsync(token, IpAddress());

            if (serviceLayerResponse.Success)
                return Ok(serviceLayerResponse.Message);
            return BadRequest(serviceLayerResponse.Message);
        }

        [HttpPost("register")]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RegisterAsync(RegisterApplicationUserRequest model, CancellationToken ct)
        {
            var serviceLayerResponse = await _applicationUserService.RegisterAsync(model, Request.Headers["origin"]);

            if (serviceLayerResponse.Success)
                // TODO: return Ok(new { message = "Verification successful, you can now login" });
                return Ok(serviceLayerResponse.Message);
            return BadRequest(serviceLayerResponse.Message);
        }

        //[HttpPost("verify-email")]
        //[ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> VerifyEmailAsync(VerifyEmailRequest model, CancellationToken ct)
        //{
        //    var serviceLayerResponse = await _applicationUserService.VerifyEmailAsync(model.Token);

        //    if (serviceLayerResponse.Success)
        //        //return Ok(new { message = "Verification successful, you can now login" });
        //        return Ok(serviceLayerResponse.Message);
        //    return BadRequest(serviceLayerResponse.Message);
        //}

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest model, CancellationToken ct)
        //{
        //    var serviceLayerResponse =
        //        await _applicationUserService.ForgotPasswordAsync(model, Request.Headers["origin"]);

        //    if (serviceLayerResponse.Success)
        //        //return Ok(new { message = "Please check your email for password reset instructions" });
        //        return Ok(serviceLayerResponse.Message);
        //    return BadRequest(serviceLayerResponse.Message);
        //}

        //[HttpPost("validate-reset-token")]
        //[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //public async Task<IActionResult> ValidateResetTokenAsync(ValidateResetTokenRequest model, CancellationToken ct)
        //{
        //    var serviceLayerResponse = await _applicationUserService.ValidateResetTokenAsync(model);

        //    if (serviceLayerResponse.Success)
        //        //  return Ok(new { message = "Token is valid" });
        //        return Ok(serviceLayerResponse.Message);
        //    return BadRequest(serviceLayerResponse.Message);
        //}

        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest model, CancellationToken ct)
        //{
        //    var serviceLayerResponse = await _applicationUserService.ResetPasswordAsync(model);

        //    if (serviceLayerResponse.Success)
        //        //  return Ok(new { message = "Password reset successful, you can now login" });
        //        return Ok(serviceLayerResponse.Message);
        //    return BadRequest(serviceLayerResponse.Message);
        //}

        ////[MyAuthorize(Role.Admin)]
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllAsync()
        //{
        //    // TODO: fix them ids (shadow them, cant be visible under DDD)
        //    // TODO: make sure field access works, try adding a role directly
        //    var t = await _applicationUserService
        //        .Queryable()
        //        .Include(e => e.CreatedBy)
        //        .Include(e => e.ModifiedBy)
        //        .Include(e => e.DeletedBy)
        //        .Include(e => e.ActivatedBy)
        //        .Include(e => e.DeactivatedBy)
        //        .Include(e => e.UserRoles)
        //        .ThenInclude(u => u.Role)
        //        .ThenInclude(u => u.CreatedBy)
        //        .Include(e => e.UserRoles)
        //        .ThenInclude(u => u.CreatedBy)
        //        .Include(u => u.RefreshTokens)
        //        .Where(user => user.Id == 1)
        //        .FirstOrDefaultAsync();


        //    var users = await _applicationUserService
        //        .Queryable()
        //        //.Where(user => user.Active)
        //        .Include(e => e.UserRoles)
        //        .ThenInclude(u => u.Role)
        //        .Include(u => u.RefreshTokens)
        //        //.ProjectTo<ApplicationUserViewModel>(configuration: _mapper.ConfigurationProvider)
        //        .Select(i => new UserViewModel
        //        {
        //            FirstName = i.FirstName,
        //            LastName = i.LastName,
        //            Active = i.Active,
        //            ActivatedBy = i.ActivatedBy != null
        //                ? new UserViewModel
        //                {
        //                    Id = i.ActivatedBy.Id,
        //                    FirstName = i.ActivatedBy.FirstName,
        //                    LastName = i.ActivatedBy.LastName,
        //                    Email = i.ActivatedBy.Email,
        //                    UserName = i.ActivatedBy.UserName
        //                }
        //                : null,
        //            Created = i.Created,
        //            CreatedBy = i.CreatedBy != null
        //                ? new UserViewModel
        //                {
        //                    Id = i.CreatedBy.Id,
        //                    FirstName = i.CreatedBy.FirstName,
        //                    LastName = i.CreatedBy.LastName,
        //                    Email = i.CreatedBy.Email,
        //                    UserName = i.CreatedBy.UserName
        //                }
        //                : null,
        //            DateCreated = i.DateCreated,
        //            DateModified = i.DateModified,
        //            FullName = i.FullName,
        //            IsDraft = i.IsDraft,
        //            Id = i.Id,
        //            BasicRole = i.BasicRole,
        //            Roles = i.UserRoles.Select(r => new RoleViewModel
        //            {
        //                Name = r.Role.Name,
        //                Active = r.Role.Active,
        //                CreatedBy = r.Role.CreatedBy != null
        //                    ? new UserViewModel
        //                    {
        //                        Id = r.Role.CreatedBy.Id,
        //                        FirstName = r.Role.CreatedBy.FirstName,
        //                        LastName = r.Role.CreatedBy.LastName,
        //                        Email = r.Role.CreatedBy.Email,
        //                        UserName = r.Role.CreatedBy.UserName
        //                    }
        //                    : null,
        //                DateCreated = r.Role.DateCreated,
        //                DateModified = r.Role.DateModified,
        //                IsDraft = r.Role.IsDraft,
        //                Id = r.Role.Id,
        //                ActiveFrom = r.Role.ActiveFrom,
        //                ActiveTo = r.Role.ActiveTo,
        //                ActivatedBy = r.Role.ActivatedBy != null
        //                    ? new UserViewModel
        //                    {
        //                        Id = r.Role.ActivatedBy.Id,
        //                        FirstName = r.Role.ActivatedBy.FirstName,
        //                        LastName = r.Role.ActivatedBy.LastName,
        //                        Email = r.Role.ActivatedBy.Email,
        //                        UserName = r.Role.ActivatedBy.UserName
        //                    }
        //                    : null,
        //                Deleted = r.Role.Deleted,
        //                DeletedBy = r.Role.DeletedBy != null
        //                    ? new UserViewModel
        //                    {
        //                        Id = r.Role.DeletedBy.Id,
        //                        FirstName = r.Role.DeletedBy.FirstName,
        //                        LastName = r.Role.DeletedBy.LastName,
        //                        Email = r.Role.DeletedBy.Email,
        //                        UserName = r.Role.DeletedBy.UserName
        //                    }
        //                    : null
        //            }).ToList(),
        //            PasswordReset = i.PasswordReset,
        //            PasswordResetMsg = i.PasswordResetMsg,
        //            ResetToken = i.ResetToken,
        //            ResetTokenExpires = i.ResetTokenExpires,
        //            Updated = i.Updated,
        //            //UserRoles = i.UserRoles.Where(user => user.UserId == i.Id).Select(i => new UserRoleViewModel
        //            //{
        //            //    IsActive = i.IsActive,
        //            //    //ModifiedBy = new UserViewModel
        //            //    //{
        //            //    //    Id = i.ModifiedBy.Id,
        //            //    //    FirstName = i.ModifiedBy.FirstName,
        //            //    //    LastName = i.ModifiedBy.LastName,
        //            //    //    Email = i.ModifiedBy.Email,
        //            //    //    UserName = i.ModifiedBy.UserName,
        //            //    //},
        //            //    Role = i.Role.UserRoles.Where(user => user.UserId == i.Id).Select(r => new RoleViewModel
        //            //    {
        //            //        Name = r.Role.Name,
        //            //        Active = r.Role.Active,
        //            //        //CreatedBy = new UserViewModel
        //            //        //{
        //            //        //    Id = r.Role.CreatedBy.Id,
        //            //        //    FirstName = r.Role.CreatedBy.FirstName,
        //            //        //    LastName = r.Role.CreatedBy.LastName,
        //            //        //    Email = r.Role.CreatedBy.Email,
        //            //        //    UserName = r.Role.CreatedBy.UserName,
        //            //        //},
        //            //        DateCreated = r.Role.DateCreated,
        //            //        DateModified = r.Role.DateModified,
        //            //        IsDraft = r.Role.IsDraft,
        //            //        Id = r.Role.Id,
        //            //        ActiveFrom = r.Role.ActiveFrom,
        //            //        ActiveTo = r.Role.ActiveTo,
        //            //        Deleted = r.Role.Deleted,
        //            //        //DeletedBy = new UserViewModel
        //            //        //{
        //            //        //    Id = r.Role.DeletedBy.Id,
        //            //        //    FirstName = r.Role.DeletedBy.FirstName,
        //            //        //    LastName = r.Role.DeletedBy.LastName,
        //            //        //    Email = r.Role.DeletedBy.Email,
        //            //        //    UserName = r.Role.DeletedBy.UserName,
        //            //        //},
        //            //        DateDeleted = r.Role.DateDeleted,
        //            //        //ModifiedBy = new UserViewModel
        //            //        //{
        //            //        //    Id = r.Role.ModifiedBy.Id,
        //            //        //    FirstName = r.Role.ModifiedBy.FirstName,
        //            //        //    LastName = r.Role.ModifiedBy.LastName,
        //            //        //    Email = r.Role.ModifiedBy.Email,
        //            //        //    UserName = r.Role.ModifiedBy.UserName,
        //            //        //}
        //            //    }).FirstOrDefault(),
        //            //    UserId = i.UserId,
        //            //    RoleId = i.RoleId,
        //            //    ActiveFrom = i.ActiveFrom,
        //            //    ActiveTo = i.ActiveTo,
        //            //    //CreatedBy = new UserViewModel
        //            //    //{
        //            //    //    Id = i.CreatedBy.Id,
        //            //    //    FirstName = i.CreatedBy.FirstName,
        //            //    //    LastName = i.CreatedBy.LastName,
        //            //    //    Email = i.CreatedBy.Email,
        //            //    //    UserName = i.CreatedBy.UserName,
        //            //    //},
        //            //    DateCreated = i.DateCreated,
        //            //    Id = i.Id,
        //            //    Name = i.Name,
        //            //    IsDraft = i.IsDraft,
        //            //    Description = i.Description,
        //            //    Deleted = i.Deleted,
        //            //    DateDeleted = i.DateDeleted,
        //            //    //DeletedBy = new UserViewModel
        //            //    //{
        //            //    //    Id = i.DeletedBy.Id,
        //            //    //    FirstName = i.DeletedBy.FirstName,
        //            //    //    LastName = i.DeletedBy.LastName,
        //            //    //    Email = i.DeletedBy.Email,
        //            //    //    UserName = i.DeletedBy.UserName,
        //            //    //},
        //            //    DateModified = i.DateModified
        //            //}).ToList(),
        //            VerificationToken = i.VerificationToken,
        //            Verified = i.Verified,
        //            UserName = i.UserName,
        //            Email = i.Email,
        //            VerificationTokenExpirationDate = i.VerificationTokenExpirationDate,
        //            VerificationFailureLatestMessage = i.VerificationFailureLatestMessage,
        //            UndeleteReason = i.UndeleteReason,
        //            LastVerificationFailureDate = i.LastVerificationFailureDate,
        //            DeletedBy = i.DeletedBy != null
        //                ? new UserViewModel
        //                {
        //                    Id = i.DeletedBy.Id,
        //                    FirstName = i.DeletedBy.FirstName,
        //                    LastName = i.DeletedBy.LastName,
        //                    Email = i.DeletedBy.Email,
        //                    UserName = i.DeletedBy.UserName
        //                }
        //                : null,
        //            DeleteReason = i.DeleteReason,
        //            DeactivateReason = i.DeactivateReason,
        //            DateOfBirth = i.DateOfBirth,
        //            RefreshTokens = i.RefreshTokens.Select(refreshToken => new RefreshTokenViewModel
        //            {
        //                Token = refreshToken.Token,
        //                Created = refreshToken.Created,
        //                CreatedByIp = refreshToken.CreatedByIp,
        //                Expires = refreshToken.Expires,
        //                Id = refreshToken.Id,
        //                IsExpired = refreshToken.IsExpired,
        //                ReplacedByToken = refreshToken.ReplacedByToken,
        //                Revoked = refreshToken.Revoked,
        //                RevokedByIp = refreshToken.RevokedByIp,
        //                IsActive = refreshToken.IsActive
        //            }).ToList()
        //        })
        //        .ToListAsync();

        //    return Ok(users);
        //}

        //[MyAuthorize]
        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<UserViewModel>> GetByIdAsync(int id)
        //{
        //    //// users can get their own applicationUser and admins can get any applicationUser
        //    //if (id != this.applicationUser.Id && this.applicationUser.Role != Role.Admin)
        //    //    return Unauthorized(new { message = "Unauthorized" });

        //    var user = await _applicationUserService
        //        .Queryable()
        //        //.Where(user => user.Active && user.Id == id)
        //        //.Include(e => e.UserRoles)
        //        //.ThenInclude(u => u.Role)
        //        .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
        //        .Select(i => new UserViewModel
        //        {
        //            FirstName = i.FirstName,
        //            LastName = i.LastName,
        //            Active = i.Active,
        //            ActivatedBy = i.ActivatedBy != null
        //                ? new UserViewModel
        //                {
        //                    Id = i.ActivatedBy.Id,
        //                    FirstName = i.ActivatedBy.FirstName,
        //                    LastName = i.ActivatedBy.LastName,
        //                    Email = i.ActivatedBy.Email,
        //                    UserName = i.ActivatedBy.UserName
        //                }
        //                : null,
        //            Created = i.Created,
        //            CreatedBy = i.CreatedBy != null
        //                ? new UserViewModel
        //                {
        //                    Id = i.CreatedBy.Id,
        //                    FirstName = i.CreatedBy.FirstName,
        //                    LastName = i.CreatedBy.LastName,
        //                    Email = i.CreatedBy.Email,
        //                    UserName = i.CreatedBy.UserName
        //                }
        //                : null,
        //            DateCreated = i.DateCreated,
        //            DateModified = i.DateModified,
        //            FullName = i.FullName,
        //            IsDraft = i.IsDraft,
        //            Id = i.Id,
        //            BasicRole = i.BasicRole,
        //            Roles = i.UserRoles.Select(r => new RoleViewModel
        //            {
        //                Name = r.Role.Name,
        //                Active = r.Role.Active,
        //                CreatedBy = r.Role.CreatedBy != null
        //                    ? new UserViewModel
        //                    {
        //                        Id = r.Role.CreatedBy.Id,
        //                        FirstName = r.Role.CreatedBy.FirstName,
        //                        LastName = r.Role.CreatedBy.LastName,
        //                        Email = r.Role.CreatedBy.Email,
        //                        UserName = r.Role.CreatedBy.UserName
        //                    }
        //                    : null,
        //                DateCreated = r.Role.DateCreated,
        //                DateModified = r.Role.DateModified,
        //                IsDraft = r.Role.IsDraft,
        //                Id = r.Role.Id,
        //                ActiveFrom = r.Role.ActiveFrom,
        //                ActiveTo = r.Role.ActiveTo,
        //                ActivatedBy = r.Role.ActivatedBy != null
        //                    ? new UserViewModel
        //                    {
        //                        Id = r.Role.ActivatedBy.Id,
        //                        FirstName = r.Role.ActivatedBy.FirstName,
        //                        LastName = r.Role.ActivatedBy.LastName,
        //                        Email = r.Role.ActivatedBy.Email,
        //                        UserName = r.Role.ActivatedBy.UserName
        //                    }
        //                    : null,
        //                Deleted = r.Role.Deleted,
        //                DeletedBy = r.Role.DeletedBy != null
        //                    ? new UserViewModel
        //                    {
        //                        Id = r.Role.DeletedBy.Id,
        //                        FirstName = r.Role.DeletedBy.FirstName,
        //                        LastName = r.Role.DeletedBy.LastName,
        //                        Email = r.Role.DeletedBy.Email,
        //                        UserName = r.Role.DeletedBy.UserName
        //                    }
        //                    : null
        //            }).ToList(),
        //            PasswordReset = i.PasswordReset,
        //            PasswordResetMsg = i.PasswordResetMsg,
        //            ResetToken = i.ResetToken,
        //            ResetTokenExpires = i.ResetTokenExpires,
        //            Updated = i.Updated,
        //            //UserRoles = i.UserRoles.Where(user => user.UserId == i.Id).Select(i => new UserRoleViewModel
        //            //{
        //            //    IsActive = i.IsActive,
        //            //    //ModifiedBy = new UserViewModel
        //            //    //{
        //            //    //    Id = i.ModifiedBy.Id,
        //            //    //    FirstName = i.ModifiedBy.FirstName,
        //            //    //    LastName = i.ModifiedBy.LastName,
        //            //    //    Email = i.ModifiedBy.Email,
        //            //    //    UserName = i.ModifiedBy.UserName,
        //            //    //},
        //            //    Role = i.Role.UserRoles.Where(user => user.UserId == i.Id).Select(r => new RoleViewModel
        //            //    {
        //            //        Name = r.Role.Name,
        //            //        Active = r.Role.Active,
        //            //        //CreatedBy = new UserViewModel
        //            //        //{
        //            //        //    Id = r.Role.CreatedBy.Id,
        //            //        //    FirstName = r.Role.CreatedBy.FirstName,
        //            //        //    LastName = r.Role.CreatedBy.LastName,
        //            //        //    Email = r.Role.CreatedBy.Email,
        //            //        //    UserName = r.Role.CreatedBy.UserName,
        //            //        //},
        //            //        DateCreated = r.Role.DateCreated,
        //            //        DateModified = r.Role.DateModified,
        //            //        IsDraft = r.Role.IsDraft,
        //            //        Id = r.Role.Id,
        //            //        ActiveFrom = r.Role.ActiveFrom,
        //            //        ActiveTo = r.Role.ActiveTo,
        //            //        Deleted = r.Role.Deleted,
        //            //        //DeletedBy = new UserViewModel
        //            //        //{
        //            //        //    Id = r.Role.DeletedBy.Id,
        //            //        //    FirstName = r.Role.DeletedBy.FirstName,
        //            //        //    LastName = r.Role.DeletedBy.LastName,
        //            //        //    Email = r.Role.DeletedBy.Email,
        //            //        //    UserName = r.Role.DeletedBy.UserName,
        //            //        //},
        //            //        DateDeleted = r.Role.DateDeleted,
        //            //        //ModifiedBy = new UserViewModel
        //            //        //{
        //            //        //    Id = r.Role.ModifiedBy.Id,
        //            //        //    FirstName = r.Role.ModifiedBy.FirstName,
        //            //        //    LastName = r.Role.ModifiedBy.LastName,
        //            //        //    Email = r.Role.ModifiedBy.Email,
        //            //        //    UserName = r.Role.ModifiedBy.UserName,
        //            //        //}
        //            //    }).FirstOrDefault(),
        //            //    UserId = i.UserId,
        //            //    RoleId = i.RoleId,
        //            //    ActiveFrom = i.ActiveFrom,
        //            //    ActiveTo = i.ActiveTo,
        //            //    //CreatedBy = new UserViewModel
        //            //    //{
        //            //    //    Id = i.CreatedBy.Id,
        //            //    //    FirstName = i.CreatedBy.FirstName,
        //            //    //    LastName = i.CreatedBy.LastName,
        //            //    //    Email = i.CreatedBy.Email,
        //            //    //    UserName = i.CreatedBy.UserName,
        //            //    //},
        //            //    DateCreated = i.DateCreated,
        //            //    Id = i.Id,
        //            //    Name = i.Name,
        //            //    IsDraft = i.IsDraft,
        //            //    Description = i.Description,
        //            //    Deleted = i.Deleted,
        //            //    DateDeleted = i.DateDeleted,
        //            //    //DeletedBy = new UserViewModel
        //            //    //{
        //            //    //    Id = i.DeletedBy.Id,
        //            //    //    FirstName = i.DeletedBy.FirstName,
        //            //    //    LastName = i.DeletedBy.LastName,
        //            //    //    Email = i.DeletedBy.Email,
        //            //    //    UserName = i.DeletedBy.UserName,
        //            //    //},
        //            //    DateModified = i.DateModified
        //            //}).ToList(),
        //            VerificationToken = i.VerificationToken,
        //            Verified = i.Verified,
        //            UserName = i.UserName,
        //            Email = i.Email,
        //            VerificationTokenExpirationDate = i.VerificationTokenExpirationDate,
        //            VerificationFailureLatestMessage = i.VerificationFailureLatestMessage,
        //            UndeleteReason = i.UndeleteReason,
        //            LastVerificationFailureDate = i.LastVerificationFailureDate,
        //            DeletedBy = i.DeletedBy != null
        //                ? new UserViewModel
        //                {
        //                    Id = i.DeletedBy.Id,
        //                    FirstName = i.DeletedBy.FirstName,
        //                    LastName = i.DeletedBy.LastName,
        //                    Email = i.DeletedBy.Email,
        //                    UserName = i.DeletedBy.UserName
        //                }
        //                : null,
        //            DeleteReason = i.DeleteReason,
        //            DeactivateReason = i.DeactivateReason,
        //            DateOfBirth = i.DateOfBirth,
        //            RefreshTokens = i.RefreshTokens.Select(refreshTokenViewModel => new RefreshTokenViewModel
        //            {
        //                Token = refreshTokenViewModel.Token,
        //                Created = refreshTokenViewModel.Created,
        //                CreatedByIp = refreshTokenViewModel.CreatedByIp,
        //                Expires = refreshTokenViewModel.Expires,
        //                Id = refreshTokenViewModel.Id,
        //                IsExpired = refreshTokenViewModel.IsExpired,
        //                ReplacedByToken = refreshTokenViewModel.ReplacedByToken,
        //                Revoked = refreshTokenViewModel.Revoked,
        //                RevokedByIp = refreshTokenViewModel.RevokedByIp,
        //                IsActive = refreshTokenViewModel.IsActive
        //            }).ToList()
        //        })
        //        .SingleAsync();

        //    return Ok(user);
        //}

        #region Private props

        private readonly MyConfigurationValues _configurationValues;
        private readonly IUserService _applicationUserService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITracer _tracer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        #endregion Private props

        #region Public props

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string FirstName { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string LastName { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string Email { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable 1591
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public string UserName { get; private set; }
#pragma warning restore 1591

        #endregion Public props

        #region CUD non CQRS

        [MyAuthorize(RoleEnum.Admin)]
        [HttpPost]
        [Produces(typeof(ApplicationUserResponse))]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult<ApplicationUserResponse>> CreateAsync(CreateApplicationUserRequest model,
            CancellationToken ct)
        {
            var serviceLayerResponse = await _applicationUserService.CreateAsync(model,
                (User) _contextAccessor.HttpContext.Items["ApplicationUser"]);

            if (!serviceLayerResponse.Success)
                return BadRequest(serviceLayerResponse.Message);
            return CreatedAtAction(nameof(CreateAsync), new {id = serviceLayerResponse.ViewModel.Id},
                serviceLayerResponse.ViewModel);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<ApplicationUserResponse>> UpdateAsync(int id, UpdateApplicationUserRequest model,
            CancellationToken ct)
        {
            //// users can update their own applicationUser and admins can update any applicationUser
            //if (id != this.applicationUser.Id && this.applicationUser.Role != Role.Admin)
            //    return Unauthorized(new { message = "Unauthorized" });

            // only admins can update role
            //if (this.applicationUser.Role != Role.Admin)
            //    model.Role = null;

            var serviceLayerResponse = await _applicationUserService.UpdateAsync(id, model,
                (User) _contextAccessor.HttpContext.Items["ApplicationUser"]);

            if (!serviceLayerResponse.Success)
                return BadRequest(serviceLayerResponse.Message);
            return Ok(serviceLayerResponse.ViewModel);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ApplicationUserResponse>> DeleteAsync(int id, CancellationToken ct)
        {
            //// users can delete their own applicationUser and admins can delete any applicationUser
            //if (id != applicationUser.Id && applicationUser.Role != Role.Admin)
            //    return Unauthorized(new { message = "Unauthorized" });

            var serviceLayerResponse =
                await _applicationUserService.DeleteAsync(id,
                    (User) _contextAccessor.HttpContext.Items["ApplicationUser"]);

            if (!serviceLayerResponse.Success)
                return BadRequest(serviceLayerResponse.Message);
            return Ok(serviceLayerResponse.ViewModel);
        }

        #endregion CUD non CQRS

        #region CUD CQRS

        [HttpPost("register-cqrs")]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RegisterCqrsAsync(RegisterApplicationUserRequest request, CancellationToken ct)
        {
            // As an example of a non-trivial event based flow we got the following:

            // [RegisterUserCommand] will get handled by [RegisterApplicationUserCommandHandler]
            // that one will fire an event [ApplicationUserCreatedDomainEvent] (when the user is created, the user has not yet confirmed his email so his account is not yet fully activated)
            // this will get handled by [ApplicationUserCreatedDomainEventHandler]
            // this will fire [SendAccountVerificationMailCommand]
            // this will get handled by [SendAccountVerificationMailCommandHandler] that will send an email containing the activation link
            // the flow continues when the user clicks on the provided url (found in an email that we had sent to the user)
            // [EmailVerifiedDomainEvent] is then fired, and is handled by [EmailVerifiedDomainEventHandler] and the user will be marked as "verified"
            // finally a greeting mail is sent to the user
            // this will fire a new command: [SendAWelcomeMailCommand]
            // this command will get handled by [SendAWelcomeMailCommandHandler], that will send a greeting email to the users email address
            // a domain event is fired in response to this => [MarkApplicationUserAsWelcomedDomainEvent] is sent
            // and is handled by [MarkApplicationUserAsWelcomedDomainEventHandler] marking the user as "welcomed"

            // this flow if naturally extended by the "forgot password" flow and the "resend activation link" flow, both of which supplement the "happy road" flow with logical contingencies
            // => Forgot password Flow
            // => Resend activation link Flow

            #region For development purposes this is unfortunately needed

            var creator = (User) _contextAccessor.HttpContext.Items["ApplicationUser"];
            long creatorId = -1;
            if (creator == null)
            {
                if (_configurationValues.Environment.Trim().ToUpper() == ApplicationConstants.DEVELOPMENT ||
                    _configurationValues.Environment.Trim().ToUpper() == ApplicationConstants.LOCALDEVELOPMENT)
                    creatorId = 1; // User 1 is a "system" user
            }
            else
            {
                creatorId = creator.Id;
            }

            #endregion For development purposes this is unfortunately needed

            using var scope = _tracer.BuildSpan("RegisterCqrsAsync").StartActive(true);
            var user = await _mediator.Send(new RegisterUserCommand(
                request.Email
                , request.ConfirmPassword
                , request.DateOfBirth
                , request.FirstName
                , request.LastName
                , request.Password
                , request.UserName
                , request.Oib
                , creatorId
            ), ct);

            return Created(string.Empty, user);
        }

        //[MyAuthorize(Role.Admin)]
        [HttpPost("create-cqrs")]
        [Produces(typeof(ApplicationUserResponse))]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int) HttpStatusCode.Created)]
        public async Task<ActionResult<UserDto>> CreateCqrsAsync([FromBody] CreateApplicationUserRequest request,
            CancellationToken ct)
        {
            using var scope = _tracer.BuildSpan("CreateCqrsAsync").StartActive(true);
            var user = await _mediator.Send(new CreateUserCommand(
                request.Email
                , request.ConfirmPassword
                , request.DateOfBirth
                , request.FirstName
                , request.LastName
                , request.Password
                , request.Role
                , request.Title
                , request.UserName
                , (User) _contextAccessor.HttpContext.Items["ApplicationUser"]
            ), ct);

            return Created(string.Empty, user);
        }

        [HttpPost("assign-role-to-user-cqrs")]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AssignRoleToUserCqrsAsync(AssignRoleToUserCommand command,
            CancellationToken ct)
        {
            using var scope = _tracer.BuildSpan("AssignRoleToUserCqrsAsync").StartActive(true);
            command.AssignerUser = (User) _contextAccessor.HttpContext.Items["ApplicationUser"];
            await _mediator.Send(command, ct);

            return Ok();
        }

        [HttpPost("remove-role-from-user-cqrs")]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RemoveRoleFromUserCqrsAsync(RemoveRoleFromUserCommand command,
            CancellationToken ct)
        {
            using var scope = _tracer.BuildSpan("RemoveRoleFromUserCqrsAsync").StartActive(true);
            command.RemoverUser = (User) _contextAccessor.HttpContext.Items["ApplicationUser"];
            await _mediator.Send(command, ct);

            return Ok();
        }


        [HttpPost("assign-address-cqrs")]
        [Produces(typeof(AssignAddressToUserResponse))]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<ActionResult<UserDto>> AssignAddressToUserCqrsAsync(
            [FromBody] AssignAddressToUserRequest request,
            CancellationToken ct)
        {
            using var scope = _tracer.BuildSpan("AssignAddressToUserCqrsAsync").StartActive(true);
            var user = await _mediator.Send(new AssignAddressToUserCommand(
                request.UserId
                , request.AddressTypeName
                , request.AddressName
                , request.Description
                , request.Line1
                , request.Line2
                , request.FlatNr
                , request.PostalCode
                , request.HouseNumber
                , request.HouseNumberSuffix
                , request.UserComment
                , request.CityBlockName
                , request.CountryName
                , request.TownName
                , request.CountyName
                , (User) _contextAccessor.HttpContext.Items["ApplicationUser"]
                , request.ActiveFrom
                , request.ActiveTo
            ), ct);

            return Ok("Address assigned to user.");
        }

        [HttpPost("remove-address-from-user-cqrs")]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType((int) HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RemoveAddressFromUserCqrsAsync(RemoveAddressFromUserCommand command,
            CancellationToken ct)
        {
            using var scope = _tracer.BuildSpan("RemoveAddressFromUserCqrsAsync").StartActive(true);
            command.RemoverUser = (User) _contextAccessor.HttpContext.Items["ApplicationUser"];
            await _mediator.Send(command, ct);

            return Ok();
        }


        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update-cqrs/{id:int}")]
        public async Task<ActionResult<ApplicationUserResponse>> UpdateCqrsAsync(int id,
            [FromBody] UpdateApplicationUserRequest model, CancellationToken ct)
        {
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete-cqrs/{id:int}")]
        public async Task<ActionResult<ApplicationUserResponse>> DeleteCqrsAsync(int id, CancellationToken ct)
        {
            return null;
        }

        #endregion CUD CQRS

        #region Other

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("password-reset-cqrs/{id:int}")]
        public async Task<ActionResult<ApplicationUserResponse>> PasswordResetCqrsAsync(int id, CancellationToken ct)
        {
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("resend-activation-link-cqrs/{id:int}")]
        public async Task<ActionResult<ApplicationUserResponse>> ResendActivationLinkCqrsAsync(int id,
            CancellationToken ct)
        {
            return null;
        }

        [HttpPost("verify-email-cqrs")]
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> VerifyEmailCQRSAsync(VerifyEmailRequest model, CancellationToken ct)
        {
            var serviceLayerResponse =
                await _applicationUserService.VerifyEmailAsync(model.Token, Request.Headers["origin"]);

            if (serviceLayerResponse.Success)
                //return Ok(new { message = "Verification successful, you can now login" });
                return Ok(serviceLayerResponse.Message);
            return BadRequest(serviceLayerResponse.Message);
        }

        [HttpPost("forgot-password-cqrs")]
        public async Task<IActionResult> ForgotPasswordCQRSAsync(ForgotPasswordRequest model, CancellationToken ct)
        {
            var serviceLayerResponse =
                await _applicationUserService.ForgotPasswordAsync(model, Request.Headers["origin"]);

            if (serviceLayerResponse.Success)
                //return Ok(new { message = "Please check your email for password reset instructions" });
                return Ok(serviceLayerResponse.Message);
            return BadRequest(serviceLayerResponse.Message);
        }

        [HttpPost("validate-reset-token-cqrs")]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        [ProducesResponseType((int) HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> ValidateResetTokenCQRSAsync(ValidateResetTokenRequest model,
            CancellationToken ct)
        {
            var serviceLayerResponse = await _applicationUserService.ValidateResetTokenAsync(model);

            if (serviceLayerResponse.Success)
                //  return Ok(new { message = "Token is valid" });
                return Ok(serviceLayerResponse.Message);
            return BadRequest(serviceLayerResponse.Message);
        }

        #endregion Other


        #region Helpers

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        #endregion Helpers
    }
}