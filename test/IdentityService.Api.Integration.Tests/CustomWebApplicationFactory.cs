using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityService.Api;
using IdentityService.Application.DomainServices.EmailServices;
using IdentityService.Data.DatabaseContexts;
using IdentityService.Data.Seed;
using IdentityService.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SharedKernel.Helpers.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Helpers.EmailSender;

namespace IdentityService.Tests.Integration.Fixtures
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public IServiceProvider Services { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override IHostBuilder CreateHostBuilder()
        {
            var connStr = "DataSource=InMemoryDb;Mode=Memory;Cache=Shared";

            // Build the host builder similar to your Program.cs
            var builder = Host.CreateDefaultBuilder()
                .UseEnvironment("Test")
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();

                    Configuration = config.Build();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostingContext, services) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    // Remove the app's ApplicationDbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Use SQLite in-memory database
                    var sqliteConnection = new SqliteConnection(connStr);
                    sqliteConnection.Open();

                    // Add ApplicationDbContext using SQLite in-memory database
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlite(sqliteConnection);
                        options.EnableSensitiveDataLogging();
                        options.EnableDetailedErrors();
                    });

                    // Register other services from Startup.cs
                    RegisterServicesFromStartup(services);

                    // Remove or adjust services that may cause issues in tests
                    RemoveServicesNotNeededInTests(services);

                    // Replace external dependencies with mocks
                    services.AddSingleton<IEmailService, MockEmailService>();
                })
                .ConfigureContainer<ContainerBuilder>((hostingContext, containerBuilder) =>
                {
                    var env = hostingContext.HostingEnvironment;

                    // Configure Autofac container
                    TestBootstrap.ConfigureContainer(containerBuilder, Configuration, env);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return builder;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();

            host.Start();

            // Initialize the database
            using (var scope = host.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();

                // Seed data if necessary
                var databaseInitializer = scopedServices.GetRequiredService<DatabaseInitializer>();
                databaseInitializer.MigrateAndSeed();
            }

            return host;
        }

        private void RegisterServicesFromStartup(IServiceCollection services)
        {
            // Add Options
            services.AddOptions();

            // Configure MyConfigurationValues
            services.Configure<MyConfigurationValues>(Configuration.GetSection("MyConfigurationValues"));
            services.AddScoped(cfg => cfg.GetService<Microsoft.Extensions.Options.IOptionsSnapshot<MyConfigurationValues>>().Value);

            // Add IHttpContextAccessor
            services.AddHttpContextAccessor();

            // Add AutoMapper
            services.AddAutoMapper(typeof(Startup));

            // Add MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));

            // Add FluentValidation
            services.AddMvc()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssembly(Assembly.Load("IdentityService.Application"));
                    fv.ImplicitlyValidateChildProperties = true;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ConstructorHandling = Newtonsoft.Json.ConstructorHandling.AllowNonPublicDefaultConstructor;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                });

            // Add FluentValidation Client-side adapters
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            // Configure Request Localization
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("hr-HR");
            });
            CultureInfo.CurrentCulture = new CultureInfo("hr-HR");

            // Add Authentication
            services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            // Add Authorization
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder("Test")
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Configure Data Protection Token Provider Options
            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(24));

            // Add Controllers
            services.AddControllers();

            // Add Health Checks
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy());

            // Register other necessary services
            services.AddScoped<DatabaseInitializer>();

            // Configure SmtpOptions and MailOptions
            services.Configure<SmtpOptions>(Configuration.GetSection(nameof(SmtpOptions)));
            services.Configure<MailOptions>(Configuration.GetSection(nameof(MailOptions)));
            services.Configure<JwtIssuerOptions>(Configuration.GetSection(nameof(JwtIssuerOptions)));

            // Add any other services registered in your Startup.cs
            // For example:
            // services.AddSingleton<ISomeService, SomeServiceImplementation>();
            // services.AddScoped<IOtherService, OtherServiceImplementation>();
        }

        private void RemoveServicesNotNeededInTests(IServiceCollection services)
        {
            // Remove Quartz hosted services
            var quartzHostedService = services.FirstOrDefault(d =>
                d.ImplementationType?.Name == "QuartzHostedService");
            if (quartzHostedService != null)
            {
                services.Remove(quartzHostedService);
            }

            // Remove HealthChecks UI services
            var healthCheckUIAssembly = typeof(HealthChecks.UI.Configuration.Options).Assembly;
            var descriptorsToRemove = services.Where(
                d => d.ServiceType == typeof(IHostedService) &&
                     d.ImplementationType != null &&
                     d.ImplementationType.Assembly == healthCheckUIAssembly
            ).ToList();

            foreach (var descriptor in descriptorsToRemove)
            {
                services.Remove(descriptor);
            }

            // Remove or adjust other services as needed
            // For example, remove or mock external dependencies like Kafka, Consul, etc.
        }
    }
}
