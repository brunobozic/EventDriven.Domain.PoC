using AutoMapper;
using EventDriven.Domain.PoC.Application.DomainServices.UserServices;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using EventDriven.Domain.PoC.SharedKernel.ViewModelPagination;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers.BaseControllerType
{
    public class BaseController : ControllerBase
    {
        private readonly IUserService _applicationUserService;

        /// <summary>
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="mapper"></param>
        /// <param name="configurationValues"></param>
        /// <param name="memCache"></param>
        /// <param name="contextAccessor"></param>
        /// <param name="applicationUserService"></param>
        public BaseController(
            IMyUnitOfWork unitOfWork
            , IMapper mapper
            , IOptions<MyConfigurationValues> configurationValues
            , IMemoryCache memCache
            , IHttpContextAccessor contextAccessor
            , IUserService applicationUserService
        )
        {
            ConfigurationValues = configurationValues.Value;
            UnitOfWork = unitOfWork;
            ContextAccessor = contextAccessor;
            Mapper = mapper;
            MemCache = memCache;
            _applicationUserService = applicationUserService;
        }

        /// <summary>
        /// </summary>
        public IMapper Mapper { get; }

        /// <summary>
        /// </summary>
        public IMemoryCache MemCache { get; }

        public MyConfigurationValues ConfigurationValues { get; }

        public IHttpContextAccessor ContextAccessor { get; }

        public IMyUnitOfWork UnitOfWork { get; }

        // returns the current authenticated ApplicationUser (null if not logged in)
        public User ApplicationUser => (User)HttpContext.Items["ApplicationUser"];

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
}