using Autofac;
using AutoMapper;
using IdentityService.Application.AutomapperMaps;
using IdentityService.Application.DomainServices.CountryService;
using IdentityService.Data.CustomUnitOfWork;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContexts;
using IdentityService.Data.DatabaseContexts.Interfaces;
using MailKit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using URF.Core.Abstractions;
using URF.Core.Abstractions.Trackable;
using URF.Core.EF;
using URF.Core.EF.Trackable;

namespace IdentityService.API.REST.Test.Integration;



public class TestStartup
{
    public TestStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configure your services here, similar to your Program.cs or Startup.cs
        // For example, using in-memory database for EF Core
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("InMemoryDbForTesting"));
        services.AddTransient<ApplicationDbContext, ApplicationDbContext>();
        services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        // services.AddTransient<IEntityBase, BaseEntity>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Add other services needed for testing
    }

    private void RegisterRepositories(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterGeneric(typeof(TrackableRepository<>)).As(typeof(ITrackableRepository<>))
            .InstancePerRequest();
    }

    // If you're using ConfigureContainer in your main Startup class, replicate that logic here as needed
    public void ConfigureContainer(ContainerBuilder containerBuilder)
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

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline, similar to your Program.cs or Startup.cs
    }
}