using AutoMapper;
using IdentityService.Api.Controllers.BaseControllerType;
using IdentityService.Application.DomainServices.UserServices;
using IdentityService.Application.Ports.Input.Contracts;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SharedKernel.Helpers.Configuration;

namespace IdentityService.Api.Controllers;

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