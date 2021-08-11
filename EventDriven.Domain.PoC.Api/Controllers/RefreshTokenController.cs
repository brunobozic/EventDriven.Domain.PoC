using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EventDriven.Domain.PoC.Api.Rest.Attributes;
using EventDriven.Domain.PoC.Api.Rest.Controllers.BaseControllerType;
using EventDriven.Domain.PoC.Application.DomainServices.RefreshTokenServices;
using EventDriven.Domain.PoC.Application.DomainServices.UserServices;
using EventDriven.Domain.PoC.Application.Ports.Input.Contracts;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenTracing;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("refresh-token")]
    public class RefreshTokenController : BaseController, IRefreshTokenController
    {
        #region ctor

        public RefreshTokenController(
            IUserService applicationUserService,
            IRefreshTokenService refreshTokenService,
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
            _refreshTokenService = refreshTokenService;
            _configurationValues = configurationValues.Value;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _mediator = mediator;
            _tracer = tracer;
        }

        #endregion ctor

        //[HttpPost("refresh-token")]
        //[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //public async Task<ActionResult<AuthenticateResponse>> RefreshTokenAsync()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];

        //    if (string.IsNullOrEmpty(refreshToken))
        //    {
        //        return BadRequest(new { message = "Refresh EmailVerificationToken (Request cookie) is required" });
        //    }

        //    var serviceLayerResponse = await _applicationUserService.RefreshTheTokenAsync(refreshToken, IpAddress());

        //    if (serviceLayerResponse.Success)
        //    {
        //        SetTokenCookie(serviceLayerResponse.RefreshToken);

        //        return Ok(serviceLayerResponse.Message);
        //    }
        //    else
        //    {
        //        return BadRequest(serviceLayerResponse.Message);
        //    }
        //}

        //[Authorize]
        //[HttpPost("revoke-token")]
        //[ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        //public async Task<ActionResult<RevokeTokenResponse>> RevokeToken(RevokeTokenRequest model, CancellationToken ct)
        //{
        //    // accept token from request body or cookie
        //    var token = model.EmailVerificationToken ?? Request.Cookies["refreshToken"];

        //    if (string.IsNullOrEmpty(token))
        //        return BadRequest("EmailVerificationToken is required");

        //    //// users can revoke their own tokens and admins can revoke any tokens
        //    //if (!applicationUser.OwnsToken(token) && applicationUser.Role != Role.Admin)
        //    //    return Unauthorized("Unauthorized");

        //    var serviceLayerResponse = await _applicationUserService.RevokeTokenAsync(token, IpAddress());

        //    if (serviceLayerResponse.Success)
        //    {
        //        return Ok(serviceLayerResponse.Message);
        //    }
        //    else
        //    {
        //        return BadRequest(serviceLayerResponse.Message);
        //    }
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefreshTokenViewModel>>> GetAllAsync()
        {
            var refreshTokens = await _refreshTokenService
                .Queryable()
                .ProjectTo<RefreshTokenViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(refreshTokens);
        }

        [MyAuthorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<RefreshTokenViewModel>> GetByUserIdAsync(Guid userId)
        {
            var refreshToken = await _refreshTokenService
                .Queryable()
                .Where(rt => rt.ApplicationUser.Id == userId)
                .ProjectTo<RefreshTokenViewModel>(_mapper.ConfigurationProvider)
                .SingleAsync();

            return Ok(refreshToken);
        }

        #region CUD non CQRS

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<RefreshTokenResponse>> DeleteAsync(int tokenId, CancellationToken ct)
        {
            var serviceLayerResponse = await _refreshTokenService.DeleteAsync(tokenId,
                (User) _contextAccessor.HttpContext.Items["ApplicationUser"]);

            if (!serviceLayerResponse.Success)
                return BadRequest(serviceLayerResponse.Message);
            return Ok(serviceLayerResponse.ViewModel);
        }

        #endregion CUD non CQRS

        #region private props

        private readonly MyConfigurationValues _configurationValues;
        private readonly IUserService _applicationUserService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITracer _tracer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        #endregion private props

        #region public props

        #endregion public props

        #region CUD CQRS

        #endregion CUD CQRS

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