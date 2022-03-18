using AutoMapper;
using EventDriven.Domain.PoC.Api.Rest.Controllers.BaseControllerType;
using EventDriven.Domain.PoC.Application.DomainServices.UserServices;
using EventDriven.Domain.PoC.Application.Ports.Input.Contracts;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    [Route("addressAdministration")]
    [ApiController]
    public class AddressAdministrationController : BaseController, IAddressAdministrationController
    {
        #region ctor

        public AddressAdministrationController(
            IMyUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<MyConfigurationValues> configurationValues,
            IMemoryCache memCache,
            IHttpContextAccessor contextAccessor,
            IUserService applicationUserService,
            IAddressAdministrationService addressAdministrationService
        ) : base(unitOfWork, mapper, configurationValues, memCache, contextAccessor, applicationUserService)
        {
        }

        #endregion ctor
    }
}