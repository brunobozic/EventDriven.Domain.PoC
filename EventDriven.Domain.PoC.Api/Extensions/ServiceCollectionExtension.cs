﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrackableEntities.Common.Core;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF.Trackable;

namespace EventDriven.Domain.PoC.Api.Rest.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            var models = GetAllModels();
            services.AddScoped<DbContext, ApplicationDbContext>();

            foreach (var model in models)
            {
                var repositoryInterface = typeof(ITrackableRepository<>);
                repositoryInterface.MakeGenericType(model);
                var repositoryImplementation = typeof(TrackableRepository<>);
                repositoryImplementation.MakeGenericType(model);

                services.AddScoped(repositoryInterface, repositoryImplementation);
            }
        }

        public static void AddUrlHelper(this IServiceCollection services)
        {
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;

                return actionContext != null ? new UrlHelper(actionContext) : null;
            });
        }

        private static IEnumerable<Type> GetAllModels()
        {
            var assembly = Assembly.GetAssembly(typeof(User));
            var trackableInterface = typeof(ITrackable);

            return assembly.GetTypes().Where(t => trackableInterface.IsAssignableFrom(t)).ToList();
        }
    }
}