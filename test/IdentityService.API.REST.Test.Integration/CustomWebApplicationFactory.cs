
using Autofac;
using AutoMapper;
using IdentityService.Application.AutomapperMaps;
using IdentityService.Application.DomainServices.CountryService;
using IdentityService.Data.CustomUnitOfWork;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContexts;
using MailKit;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF.Trackable;

namespace IdentityService.API.REST.Test.Integration;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            builder.UseEnvironment("Test");
            builder.UseStartup<TestStartup>(); // Use the custom startup class for tests
        });

        builder.ConfigureTestContainer<ContainerBuilder>(containerBuilder =>
        {
            // Invoke your method or directly configure Autofac here
            ConfigureAutofac(containerBuilder);
        });
    }

    protected virtual void ConfigureAutofac(ContainerBuilder containerBuilder)
    {

        containerBuilder.RegisterType<CountryService>().As<ICountryService>().InstancePerRequest();


        containerBuilder.RegisterType<MailService>().As<IMailService>().InstancePerRequest();
        // containerBuilder.RegisterType<MailkitSendEmailJob>().As<IMailkitSendEmailJob>().InstancePerRequest();
        containerBuilder.RegisterType<MyUnitOfWork>().As<IMyUnitOfWork>().InstancePerRequest();

        RegisterRepositories(containerBuilder);

        // MediatR setup
        containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .AsClosedTypesOf(typeof(IRequestHandler<,>)).AsImplementedInterfaces();
        containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .AsClosedTypesOf(typeof(INotificationHandler<>)).AsImplementedInterfaces();

        // AutoMapper configuration
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new DomainToViewModelMappingProfile());
            cfg.AddProfile(new ViewModelToDomainMappingProfile());
        });
        var mapper = mapperConfig.CreateMapper();
        containerBuilder.RegisterInstance(mapper).As<IMapper>();

        // Example: Registering a specific DbContext options for testing
        containerBuilder.Register(context =>
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase("SafeRemoveTestDb");

            var mediator = context.Resolve<IMediator>();
            return new ApplicationDbContext(optionsBuilder.Options);
        }).AsSelf().InstancePerRequest();
    }

    private void RegisterRepositories(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterGeneric(typeof(TrackableRepository<>)).As(typeof(ITrackableRepository<>))
            .InstancePerRequest();
    }
}