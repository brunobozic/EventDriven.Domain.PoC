using AutoMapper;
using EventDriven.Domain.PoC.Api.Rest.Controllers.BaseControllerType;
using EventDriven.Domain.PoC.Application.DomainServices.UserServices;
using EventDriven.Domain.PoC.Application.Ports.Input.Contracts;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenTracing;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
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
}