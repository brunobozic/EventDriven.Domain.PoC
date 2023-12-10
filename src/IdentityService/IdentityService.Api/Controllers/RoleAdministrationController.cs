using AutoMapper;
using IdentityService.Api.Controllers.BaseControllerType;
using IdentityService.Application.DomainServices.UserServices;
using IdentityService.Application.Ports.Input.Contracts;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenTracing;
using SharedKernel.Helpers.Configuration;

namespace IdentityService.Api.Controllers;

[ApiController]
[Route("roleAdministration")]
public class RoleAdministrationController : BaseController, IRoleAdministrationController
{
    #region ctor

    public RoleAdministrationController(
        IUserService applicationUserService,
        IRoleAdministrationService roleAdministrationService,
        IMyUnitOfWork unitOfWork
        , IOptionsSnapshot<MyConfigurationValues> configurationValues
        , IMapper mapper
        , IMediator mediator
        , IMemoryCache memCache
        , IHttpContextAccessor contextAccessor
        , ITracer tracer) : base(unitOfWork, mapper, configurationValues, memCache, contextAccessor,
        applicationUserService)
    {
        _applicationUserService = applicationUserService;
        _roleAdministrationService = roleAdministrationService;
        _configurationValues = configurationValues.Value;
        _contextAccessor = contextAccessor;
        _mapper = mapper;
        _mediator = mediator;
        _tracer = tracer;
    }

    #endregion ctor

    #region Private props

    private readonly IUserService _applicationUserService;
    private readonly MyConfigurationValues _configurationValues;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IRoleAdministrationService _roleAdministrationService;

    private readonly ITracer _tracer;

    #endregion Private props
}