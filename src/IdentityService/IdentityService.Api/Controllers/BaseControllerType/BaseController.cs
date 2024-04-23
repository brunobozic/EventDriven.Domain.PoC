using AutoMapper;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.UserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;
using SharedKernel.Helpers.Configuration;
using SharedKernel.ViewModelPagination;
using System;
using System.Collections.Generic;
using System.Text;

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
        , IConfiguration configuration)
    {
        ConfigurationValues = configurationValues.Value;
        UnitOfWork = unitOfWork;
        ContextAccessor = contextAccessor;
        Mapper = mapper;
        MemCache = memCache;
        Configuration = configuration;
    }

    // returns the current authenticated ApplicationUser (null if not logged in)
    public User ApplicationUser => (User)HttpContext.Items["ApplicationUser"];

    public MyConfigurationValues ConfigurationValues { get; }

    public IHttpContextAccessor ContextAccessor { get; }

    public IMapper Mapper { get; }

    public IMemoryCache MemCache { get; }
    public IConfiguration Configuration { get; }
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
    public static string ErrorWrapper(Exception error, string nameOfMethod)
    {
        StringBuilder sb = new();

        sb.Append($"{error.Message}: {error.InnerException?.Message} ");

        Log.ForContext("NameOfMethod", nameOfMethod ?? "N/A")
            .ForContext("Errors", sb.ToString(), true)
            .Error("An error occurred in {NameOfMethod}: {Errors}", nameOfMethod, sb.ToString());

        return sb.ToString();
    }
    public static string HandleEnvironmentBasedResponse(
        Exception ex
        , string environment
        , string nameOfMethod
        , string messageTemplate
        , string correlationId
        , string origin)
    {
        if (ex != null)
            ErrorWrapper(ex, nameOfMethod);

        switch (environment)
        {
            // this is the production level message where, unless it's a validation error, we hide the detailed reasons of the underlying error
            case "Production":
                return string.Format(messageTemplate + correlationId);
            // on the other hand this is for other environments where we find it beneficial to have detailer error details to help with debugging
            default:
                {
                    if (ex != null)
                        return "Error: " + ex?.Message + Environment.NewLine + ex?.StackTrace + Environment.NewLine +
                               ex?.InnerException?.Message + Environment.NewLine + "[ Your ticket Id is: < " +
                               correlationId +
                               " > ]" + Environment.NewLine;
                    else
                        return "Unspecified error occured." + Environment.NewLine + "[Your ticket Id is: < " +
                               correlationId +
                               " > ]" + Environment.NewLine;
                }
        }
    }
}