using System.Collections.Generic;
using AutoMapper;
using IdentityService.Application.DomainServices.UserServices;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.UserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SharedKernel.Helpers.Configuration;
using SharedKernel.ViewModelPagination;

namespace IdentityService.Api.Controllers.BaseControllerType;

public class BaseController : ControllerBase
{


    /// <summary>
    /// </summary>
    /// <param name="unitOfWork"></param>
    /// <param name="mapper"></param>
    /// <param name="configurationValues"></param>
    /// <param name="memCache"></param>
    /// <param name="contextAccessor"></param>
 
    public BaseController(
        IMyUnitOfWork unitOfWork
        , IMapper mapper
        , IOptions<MyConfigurationValues> configurationValues
        , IMemoryCache memCache
        , IHttpContextAccessor contextAccessor
 
    )
    {
        ConfigurationValues = configurationValues.Value;
        UnitOfWork = unitOfWork;
        ContextAccessor = contextAccessor;
        Mapper = mapper;
        MemCache = memCache;
     
    }

    // returns the current authenticated ApplicationUser (null if not logged in)
    public User ApplicationUser => (User)HttpContext.Items["ApplicationUser"];

    public MyConfigurationValues ConfigurationValues { get; }

    public IHttpContextAccessor ContextAccessor { get; }

    public IMapper Mapper { get; }

    public IMemoryCache MemCache { get; }

    public IMyUnitOfWork UnitOfWork { get; }

    protected PagedResult<TDto> ConvertToPagedResult<TEntity, TDto>(PagedResult<TEntity> pagedResult)
    {
        var mappedPagedResult = new PagedResult<TDto>
        {
            Count = pagedResult.Count,
            PageCount = pagedResult.PageCount
        };

        mappedPagedResult.Data = Mapper.Map<IEnumerable<TEntity>, IEnumerable<TDto>>(pagedResult.Data);

        return mappedPagedResult;
    }
}