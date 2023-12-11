using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using AutoMapper;
using CommonServiceLocator;
using Confluent.Kafka;
using EventDriven.Domain.PoC.Api.Rest.Helpers.ExceptionFilters;
using Framework.Kafka.Core;
using Framework.Kafka.Core.Contracts;
using IdentityService.Api.Controllers;
using IdentityService.Api.Extensions;
using IdentityService.Api.Filters;
using IdentityService.Api.Middleware;
using IdentityService.Api.QuartzJobs;
using IdentityService.Api.SwaggerOverrides;
using IdentityService.Application.AutomapperMaps;
using IdentityService.Application.CommandsAndHandlers.Addresses;
using IdentityService.Application.CQRSBoilerplate.Command;
using IdentityService.Application.CQRSBoilerplate.Command.Handlers;
using IdentityService.Application.CQRSBoilerplate.DomainEventDispatchers;
using IdentityService.Application.CQRSBoilerplate.UnitOfWorkImplementations;
using IdentityService.Application.DomainServices.EmailServices;
using IdentityService.Application.DomainServices.UserServices;
using IdentityService.Application.EventsAndEventHandlers.Users.CUD.Notifications;
using IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Handlers;
using IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;
using IdentityService.Application.Ports.Input.Contracts;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;
using IdentityService.Data.CustomUnitOfWork;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContext;
using IdentityService.Data.DomainEventDispatching;
using IdentityService.Data.Seed;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Quartz;
using Quartz.Impl;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using SharedKernel.Autofac;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers;
using SharedKernel.Helpers.Configuration;
using SharedKernel.Helpers.Database;
using SharedKernel.Helpers.EmailSender;
using SharedKernel.Helpers.Quartz;
using SharedKernel.Kafka.ConsumedMessagePersistors;
using SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;
using SharedKernel.Kafka.KafkaImplementions;
using Swashbuckle.AspNetCore.Filters;
using URF.Core.Abstractions.Services;
using URF.Core.Services;
using ILogger = Serilog.ILogger;

// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace IdentityService.Api;

public class Program
{
    public static readonly string Namespace = typeof(Program).Namespace;
    public static readonly string AppName = Namespace;
    public static IContainer Container { get; private set; }

    public static int Main(string[] args)
    {
        var configuration = GetConfiguration();
        Log.Logger = CreateSerilogLogger(configuration);

        try
        {
            var builder = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(
                    new AutofacServiceProviderFactory()) // Set Autofac as the service provider factory
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory())
                        .UseConfiguration(configuration)
                        .ConfigureServices((context, services) =>
                        {
                            var connStr = context.Configuration.GetConnectionString("Sqlite");
                            var environment = context.HostingEnvironment;
                            var containerBuilder = new ContainerBuilder();
                            containerBuilder.Populate(services);
                            // Place your Autofac-specific registrations in this method
                            ConfigureContainer(containerBuilder, environment, connStr);

                            ConfigureServices(services, context.Configuration);

                            
                           
                            // finally build the container itself
                            Container = containerBuilder.Build();

                            // set the service locator to the Autofac one
                            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(Container));

                            // populate the composition root container, so we can use this code later on to define scope (BeginLifetimeScope)
                            CompositionRoot.SetContainer(Container);

                            var schedulerFactory = new StdSchedulerFactory();
                            var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

                            scheduler.JobFactory = new JobFactory(Container);

                            scheduler.Start().GetAwaiter().GetResult();

                            var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();

                            var trigger =
                                TriggerBuilder
                                    .Create()
                                    .StartNow()
                                    .WithCronSchedule("0/15 * * ? * *")
                                    .Build();

                            scheduler.ScheduleJob(processOutboxJob, trigger).GetAwaiter().GetResult();

                            var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();

                            var triggerCommandsProcessing =
                                TriggerBuilder
                                    .Create()
                                    .StartNow()
                                    .WithCronSchedule("0/15 * * ? * *")
                                    .Build();

                            scheduler.ScheduleJob(processInternalCommandsJob, triggerCommandsProcessing).GetAwaiter().GetResult();

                            var processKafkaPollJob = JobBuilder.Create<KafkaPollJob>().Build();

                            var triggerKafkaPollJob =
                                TriggerBuilder
                                    .Create()
                                    .StartNow()
                                    .WithCronSchedule("0/15 * * ? * *")
                                    .Build();

                            scheduler.ScheduleJob(processKafkaPollJob, triggerKafkaPollJob).GetAwaiter().GetResult();
                        })
                        .Configure(app =>
                        {
                            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
                            var serviceOptions = app.ApplicationServices
                                .GetRequiredService<IOptions<ServiceDisvoveryOptions>>();
                            Configure(app, env, serviceOptions);
                        });
                });

            

            var host = builder.Build();

            ApplyMigrations(host);

            host.Run();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
            return 1;
        }
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var connStr = configuration.GetConnectionString("Sqlite");
        services.AddOptions();
        services.Configure<ServiceDisvoveryOptions>(configuration.GetSection("ServiceDiscovery"));

        #region MVC wireup

        services.AddMvc(opt =>
            {
                // opt.Filters.Add(typeof(ValidateFilterAttribute));
            })
            //.AddFluentValidation(fv =>
            //{
            //    fv.RegisterValidatorsFromAssembly(
            //        Assembly.Load(
            //            "IdentityService.Application")); // the assembly that houses the implemented validators
            //    // fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; // dont run MVC validators after having run the fluent ones
            //    fv.ImplicitlyValidateChildProperties =
            //        true; // fall through and validate all child elements and their child elements
            //})
            .AddNewtonsoftJson(options =>
            {
                // options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy())); // Fine tuning, enum resolving
                options.SerializerSettings.ReferenceLoopHandling =
                    ReferenceLoopHandling.Ignore; // Fine tuning, ignore circular reference problems
                options.SerializerSettings.NullValueHandling =
                    NullValueHandling.Ignore; // Fine tuning, ignore null values
                options.SerializerSettings.ConstructorHandling =
                    ConstructorHandling.AllowNonPublicDefaultConstructor;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new BadRequestObjectResult(context.ModelState);

                    // TODO: add `using System.Net.Mime;` to resolve MediaTypeNames
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    result.ContentTypes.Add(MediaTypeNames.Application.Xml);

                    return result;
                };
                options.SuppressConsumesConstraintForFormFileParameters = false;
                options.SuppressInferBindingSourcesForParameters = false;
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
                options.ClientErrorMapping[404].Link =
                    "https://httpstatuses.com/404";
            })
            .AddControllersAsServices() // as the name implies, this makes all controllers become registered with IoC just as any other class would be (by default, they are not)
    
           ; 
        #endregion MVC wireup

        #region Current culture

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = new RequestCulture("hr-HR");
        });

        CultureInfo.CurrentCulture = new CultureInfo("hr-HR");

        #endregion Current culture

        services.AddMvcCore(options =>
        {
            options.Filters.Add(typeof(ValidateFilterAttribute));
            options.Filters.Add(typeof(ValidateInputFilter));
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
        });

        #region Swagger

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "EventDriven.Domain.PoC API",
                Version = "v1",
                Description = "EventDriven.Domain.PoC API",
                //TermsOfService = new Uri(null),
                Contact = new OpenApiContact
                    { Name = "bruno.bozic", Email = "bruno.bozic@gmail.com", Url = new Uri("https://dev.local/") }
            });

            //options.AddAutoQueryable(); // this does not always work, depending on the assembly version(s)
            //Set the comments path for the swagger json and ui.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            // options.AddFluentValidationRules(); // make fluent validations visible to swagger OpenApi

            options.ExampleFilters();
            // Add the custom operation filter here
            options.OperationFilter<RandomizeRegisterUserExamplesOperationFilter>();

            var securitySchema = new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            options.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement
            {
                { securitySchema, new[] { "Bearer" } }
            };
            options.AddSecurityRequirement(securityRequirement);
            // Register the custom operation filter here
            options.OperationFilter<UserRegistrationCustomOperationFilter>();
        });
        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

        #endregion Swagger

        services.Configure<MyConfigurationValues>(configuration.GetSection("MyConfigurationValues"));
        services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<MyConfigurationValues>>().Value);

        #region Authentication

        var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));
        var key = Encoding.UTF8.GetBytes(jwtAppSettingOptions[nameof(JwtIssuerOptions.Secret)]);

        services.Configure<JwtIssuerOptions>(options =>
        {
            options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
            options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
            options.SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        });

        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = false,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = false,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtAppSettingOptions[nameof(JwtIssuerOptions.Key)]))
            };
        });

        #endregion Authentication

        #region Service Registration

        services.RegisterRepositories();
        //services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddTransient<IEmailService, GmailService>();
        // services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

        services.Configure<SmtpOptions>(configuration.GetSection(nameof(SmtpOptions)));
        services.Configure<MailOptions>(configuration.GetSection(nameof(MailOptions)));
        services.Configure<JwtIssuerOptions>(configuration.GetSection(nameof(JwtIssuerOptions)));

        //services.AddTransient<IUserService, UserService>();
        //services.AddTransient<IMyUnitOfWork, MyUnitOfWork>();
        //services.AddSingleton<IDomainEventsDispatcher, IntegrationEventDispatcher>();


        #region AD

        // Uncomment this when the AD comes into play!
        //services.AddScoped<IUserProvider, AdUserProvider>();

        #endregion AD

        services.AddSingleton(provider => Log.Logger);

        #endregion Service Registration

        #region Consul

        services.AddConsulConfig(configuration);

        #endregion Consul

        #region HealthCheck

        services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            // .AddConsul(consulOptions, AppName, HealthStatus.Unhealthy, null, TimeSpan.FromSeconds(30))
            // //.AddSqlServer(Configuration.GetConnectionString("MSSql"),
            // //   name: "EventDriven.Domain.PoC-check",
            // //   tags: new[] { "EventDriven.Domain.PoC" })
            // //.AddSqlite(connStr,
            // //    name: "sql-check",
            // //    tags: new[] { "EventDriven.Domain.PoC.Repository.EF" })
            // .AddDiskStorageHealthCheck(x => x.AddDrive("C:\\", 10_000), "Check primary disk - warning", HealthStatus.Degraded)
            // .AddDiskStorageHealthCheck(x => x.AddDrive("C:\\", 2_000), "Check primary disk - error", HealthStatus.Unhealthy)
            // .AddProcessAllocatedMemoryHealthCheck(512) // 512 MB max allocated memory
            // .AddProcessHealthCheck("ProcessName", p => p.Length > 0) // check if process is running;
            // .AddFileWritePermissionsCheck(Env.WebRootPath)
            // .AddUrlGroup(new Uri("https://localhost:5001/swagger"), name: "base URL", failureStatus: HealthStatus.Degraded)
            ;

        #endregion HealthCheck

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

        #region DbContext

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connStr, x => x.MigrationsAssembly("IdentityService.Data"));
            options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
            options.EnableSensitiveDataLogging(); // Only for local development
            options.EnableDetailedErrors(); // Only for local development
        });

        #endregion DbContext

        #region CORS

        services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        #endregion CORS

        services.Configure<DataProtectionTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromHours(24));

        services.AddHttpContextAccessor();

        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new DomainToViewModelMappingProfile());
            cfg.AddProfile(new ViewModelToDomainMappingProfile());
        });

        services.AddSingleton(sp => mapperConfiguration.CreateMapper());

        var mapper = mapperConfiguration.CreateMapper();
        // mapper.ConfigurationProvider.AssertConfigurationIsValid();
        services.AddSingleton(mapper);

        var greeterMeter = new Meter("OtPrGrYa", "1.0.0");

        // Custom ActivitySource for the application
        var greeterActivitySource = new ActivitySource("OtPrGrJa");
        var tracingOtlpEndpoint = "http://localhost:4317";
        var otel = services.AddOpenTelemetry();

        // Configure OpenTelemetry Resources with the application name
        otel.ConfigureResource(resource => resource
            .AddService("OtPrGrJa"));

        // Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
        otel.WithMetrics(metrics => metrics
                // Metrics provider from OpenTelemetry
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddMeter(greeterMeter.Name)
            // Metrics provides by ASP.NET Core in .NET 8
            //.AddMeter("Microsoft.AspNetCore.Hosting")
            //.AddMeter("Microsoft.AspNetCore.Server.Kestrel")
            //.AddPrometheusExporter()
        );

        // Add Tracing for ASP.NET Core and our custom ActivitySource and export to Jaeger
        otel.WithTracing(tracing =>
        {
            tracing.AddAspNetCoreInstrumentation();
            tracing.AddHttpClientInstrumentation();
            tracing.AddSource(greeterActivitySource.Name);
            if (!string.IsNullOrEmpty(tracingOtlpEndpoint))
                tracing.AddOtlpExporter(otlpOptions => { otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint); });
            else
                tracing.AddConsoleExporter();
        });
    }

    private static void ConfigureContainer(ContainerBuilder containerBuilder, IWebHostEnvironment environment, string connStr)
    {
        //IExecutionContextAccessor executionContextAccessor =
        //    new ExecutionContextAccessor(services.GetService<IHttpContextAccessor>());

        //containerBuilder.RegisterInstance(executionContextAccessor);

        // read values from appsettings
        var config = new ConfigurationBuilder()
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                false) // beware this will default to Production appsettings if no ENV is defined on the OS                                                                                                                            // .AddJsonFile("appsettings.local.json", true) // load local settings (usually used for local debugging sessions)  ==> this will override all the other previously loaded appsettings, so comment this out in production!
            //.AddJsonFile("appsettings.local.json", true)
            //.SetBasePath(new FileInfo(processModule.FileName).DirectoryName) // this might fail on linux
            //.SetBasePath(GetBasePath()) // this might fail on linux
            .SetBasePath(environment.ContentRootPath)
            .AddEnvironmentVariables()
            .Build();

        // bind appsetting values to a configuration class
        // make that class a singleton (application wide globally accessible, so we can access the same instance from everywhere)
        var settings = new MyConfigurationValues();
        config.GetSection("MyConfigurationValues").Bind(settings);
        containerBuilder.RegisterType<MyConfigurationValues>().As<IMyConfigurationValues>().SingleInstance();
        containerBuilder.RegisterType<MyConfigurationValues>().SingleInstance();
        containerBuilder.RegisterInstance(settings);

        #region Kafka

        // pick up config values from appsettings file and populate Kafka consumer config class
        var kafkaConsumerConfig = new ConsumerConfig
        {
            BootstrapServers = settings.KafkaConsumerSettings.BootstrapServers,
            SaslUsername = settings.KafkaConsumerSettings.SaslUsername,
            SaslPassword = settings.KafkaConsumerSettings.SaslPassword,
            GroupId = settings.KafkaConsumerSettings.GroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            ClientId = settings.KafkaConsumerSettings.ClientId,
            SaslKerberosServiceName = settings.KafkaConsumerSettings.SaslKerberosServiceName, //"Kafka",
            SslCaLocation = settings.KafkaConsumerSettings.SslCaLocation, //"ca-bundle.pem"
            EnableAutoOffsetStore = settings.KafkaConsumerSettings.EnableAutoOffsetStore,
            EnablePartitionEof = settings.KafkaConsumerSettings.EnablePartitionEof,
            // MetadataRequestTimeoutMs = settings.KafkaConsumerSettings.MetadataRequestTimeoutMs,
            // MaxPollIntervalMs = settings.KafkaConsumerSettings.MaxPollIntervalMs,
            MaxInFlight = 1,
            AutoCommitIntervalMs = 50000,
            EnableAutoCommit = settings.KafkaConsumerSettings.EnableAutoCommit
        };

        if (settings.KafkaConsumerSettings.SecurityProtocol.ToUpper() ==
            SecurityProtocolEnum.SASL_Plaintext.GetDescriptionString())
            kafkaConsumerConfig.SecurityProtocol = SecurityProtocol.SaslPlaintext;

        if (settings.KafkaConsumerSettings.SaslMechanism.ToUpper() ==
            SaslMechanismEnum.Plain.GetDescriptionString())
            kafkaConsumerConfig.SaslMechanism = SaslMechanism.Plain;

        if (settings.KafkaConsumerSettings.Debug.HasValue && settings.KafkaConsumerSettings.Debug.Value)
            kafkaConsumerConfig.Debug = "ALL";

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            if (settings.KafkaConsumerSettings.SecurityProtocol.ToUpper() ==
                SecurityProtocolEnum.Kerberos.GetDescriptionString())
                kafkaConsumerConfig.SecurityProtocol = SecurityProtocol.SaslSsl;
            if (settings.KafkaConsumerSettings.SaslMechanism.ToUpper() ==
                SaslMechanismEnum.GSSAPI.GetDescriptionString())
                kafkaConsumerConfig.SaslMechanism = SaslMechanism.Gssapi;
            kafkaConsumerConfig.SaslKerberosKeytab = settings.KafkaConsumerSettings.SaslKerberosKeytab;
            kafkaConsumerConfig.SaslKerberosPrincipal = settings.KafkaConsumerSettings.SaslKerberosPrincipal;
            kafkaConsumerConfig.SaslKerberosKinitCmd = settings.KafkaConsumerSettings.SaslKerberosKinitCmd;
            kafkaConsumerConfig.SaslKerberosServiceName = settings.KafkaConsumerSettings.SaslKerberosServiceName;
            kafkaConsumerConfig.SslCaLocation = settings.KafkaConsumerSettings.SslCaLocation;
            kafkaConsumerConfig.SslCertificateLocation = settings.KafkaConsumerSettings.SslCertificateLocation;
            kafkaConsumerConfig.SslKeyLocation = settings.KafkaConsumerSettings.SslKeyLocation;
            kafkaConsumerConfig.ApiVersionRequest = settings.KafkaConsumerSettings.ApiVersionRequest;
        }
        else // Windows and OSX
        {
            if (settings.KafkaConsumerSettings.SecurityProtocol.ToUpper() ==
                SecurityProtocolEnum.SASL_Plaintext.GetDescriptionString())
                kafkaConsumerConfig.SecurityProtocol = SecurityProtocol.SaslPlaintext;
            if (settings.KafkaConsumerSettings.SaslMechanism.ToUpper() ==
                SaslMechanismEnum.Plain.GetDescriptionString()) kafkaConsumerConfig.SaslMechanism = SaslMechanism.Plain;
        }

        var kafkaProducerConfig = new ProducerConfig
        {
            BootstrapServers = settings.KafkaProducerSettings.BootstrapServers,
            SaslUsername = settings.KafkaProducerSettings.SaslUsername,
            SaslPassword = settings.KafkaProducerSettings.SaslPassword,
            ClientId = settings.KafkaProducerSettings.ClientId,
            MaxInFlight = 1
        };

        // Kafka is not best served in multi-threaded environments, keep each producer/consumer as one instance
        containerBuilder.RegisterInstance(kafkaConsumerConfig).As<ConsumerConfig>().SingleInstance();
        containerBuilder.RegisterInstance(kafkaProducerConfig).As<ProducerConfig>().SingleInstance();

        containerBuilder.RegisterType<KafkaScheduledConsumer>().As<IKafkaScheduledConsumer>()
            .UsingConstructor(typeof(ConsumerConfig), typeof(string))
            .WithParameters(new[] { new NamedParameter("topicName", settings.KafkaConsumerSettings.KafkaTopic) })
            .SingleInstance();

        containerBuilder.RegisterType<KafkaScheduledProducer>().As<IKafkaScheduledProducer>()
            .UsingConstructor(typeof(ProducerConfig), typeof(string))
            .WithParameters(new[] { new NamedParameter("topicName", settings.KafkaLoggingProducerSettings.KafkaTopic) })
            .SingleInstance();

        containerBuilder.RegisterType<KafkaLoggingProducer>().As<IKafkaLoggingProducer>()
            .UsingConstructor(typeof(MyConfigurationValues), typeof(string))
            .WithParameters(new[]
                { new NamedParameter("topicName", settings.KafkaLoggingProducerSettings.KafkaTopic) })
            .SingleInstance();

        containerBuilder.RegisterType<ConsumedMessagePersistor>().As<IConsumedMessagePersistor>()
            //.UsingConstructor(typeof(ProducerConfig), typeof(string))
            //.WithParameters(new[] { new NamedParameter("topicName", "my-new-topic") })
            .SingleInstance();

        containerBuilder.RegisterType<KafkaPollJobController>()
            .As<IJobController>()
            .UsingConstructor(typeof(MyConfigurationValues), typeof(IKafkaScheduledConsumer),
                typeof(IConsumedMessagePersistor)
            );

        #endregion Kafka

        #region Application services

        containerBuilder.RegisterType(typeof(Mediator)).As(typeof(IMediator)).InstancePerLifetimeScope();

        containerBuilder.RegisterInstance(Log.Logger)
            .As<ILogger>()
            .SingleInstance();

        containerBuilder.RegisterType(typeof(UserService)).As(typeof(IUserService)).InstancePerLifetimeScope();
        containerBuilder.RegisterType(typeof(UserController)).As(typeof(IUserController))
            .InstancePerLifetimeScope();
        containerBuilder.RegisterType<UserController>().PropertiesAutowired();

        containerBuilder.RegisterType<UserService>().As<IUserService>();

        #endregion Application services

        #region Entity Framework

        containerBuilder.RegisterType(typeof(MyUnitOfWork)).As(typeof(IMyUnitOfWork)).InstancePerLifetimeScope();

        containerBuilder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

        containerBuilder.RegisterType<SqlConnectionFactory>()
            .As<ISqlConnectionFactory>()
            .WithParameter("connectionString", connStr)
            .InstancePerLifetimeScope();

        containerBuilder.RegisterType<StronglyTypedIdValueConverterSelector>()
            .As<IValueConverterSelector>()
            .InstancePerLifetimeScope();

        #endregion Entity Framework

        #region MediatR

        containerBuilder.RegisterSource(new ScopedContravariantRegistrationSource(
            typeof(IRequestHandler<,>)
            , typeof(INotificationHandler<>)
            // , typeof(IValidator<>)
        ));

        var mediatrOpenTypes = new[]
        {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>)
            // typeof(IValidator<>),
        };

        foreach (var mediatrOpenType in mediatrOpenTypes)
            containerBuilder
                .RegisterAssemblyTypes(typeof(CreateUserCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(mediatrOpenType)
                .AsImplementedInterfaces();

        containerBuilder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        containerBuilder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        // container.RegisterGeneric(typeof(CommandValidationBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        containerBuilder.RegisterType<IntegrationEventDispatcher>()
            .As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterAssemblyTypes(typeof(UserCreatedNotification).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IIntegrationEvent<>)).InstancePerDependency();

        containerBuilder.RegisterGenericDecorator(
            typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
            typeof(INotificationHandler<>));

        containerBuilder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerDecorator<>),
            typeof(ICommandHandler<>));

        containerBuilder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));

        containerBuilder.RegisterType<CommandsDispatcher>()
            .As<ICommandsDispatcher>()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterType<CommandsScheduler>()
            .As<ICommandsScheduler>()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerDecorator<>),
            typeof(ICommandHandler<>));

        containerBuilder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));

        containerBuilder.RegisterGenericDecorator(
            typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
            typeof(INotificationHandler<>));

        containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            .AsClosedTypesOf(typeof(IDomainEventNotification<>))
            .InstancePerDependency();

        containerBuilder.RegisterType<EmailVerifiedDomainEventHandler>()
            .As<INotificationHandler<EmailVerifiedNotification>>()
            .InstancePerDependency();

        #endregion MediatR

        #region Quartz

        // This section defines the *VERY IMPORTANT* job runners without which the app, such as it is (event driven) will not function
        // The code here basically runs background jobs that pick up internal commands and integration events from db tables and execute them
        // Therefore without these jobs, the db tables will keep filling up with rows (jobs) that will never get handled
        containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => typeof(IJob).IsAssignableFrom(x))
            .InstancePerDependency();

        #endregion Quartz

        
    }

    private static void Configure(IApplicationBuilder app, IWebHostEnvironment env,
        IOptions<ServiceDisvoveryOptions> serviceOptions)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
        }
        else
        {
            app.UseDeveloperExceptionPage();
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //_ = DbInitializer.InitializeAsync(myDbContext);

        #region Swagger wireup

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            // c.RoutePrefix = string.Empty;
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.InjectStylesheet("/css/swagger.css");
        });

        #endregion Swagger wireup

        #region HealthCheck

        app.UseHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });

        #endregion HealthCheck

        app.UseStaticFiles();

        #region CORS

        app.UseCors(builder =>
            builder
                .AllowAnyOrigin()
                .WithOrigins("https://localhost:5001"
                    , "http://localhost:5000"
                    , "http://localhost:4200"
                    , "http://localhost:4205")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
        );

        #endregion CORS

        #region Global exception handling

        //The idea here is to hijack all exceptions, and decide whether to show the full trace(for developers) or to show a cleaned up
        // safe, user friendly messages to end users(commonly this is done in Production)
        app.UseExceptionHandler(
            builder =>
            {
                builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                        context.Response.Headers.Add("X-Correlation-Id", context.TraceIdentifier);

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            var json = new Startup.JsonErrorResponse
                            {
                                Messages = new[] { error.Error.Message },
                                User = context.User.Identity.Name
                            };

                            if (error.Error.Message.Contains("AD User"))
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                            if (env.EnvironmentName == "Development" || env.EnvironmentName == "LocalDevelopment")
                                json.DeveloperMessage = error.Error?.StackTrace;

                            var payload = JsonConvert.SerializeObject(json);

                            await context.Response.WriteAsync(payload);

                            Log.Error("{0} for identity: [ {1} ]", error.Error.Message, context.User.Identity.Name);
                        }
                    });
            });

        #endregion Global exception handling

        app.UseHttpsRedirection();
        app.UseRouting();

        #region AD

        // Uncomment this when AD comes into play!
        //app.UseAdMiddleware();

        #endregion AD

        #region Middlewares

        app.UseMiddleware<AddCorrelationIdToLogContextMiddleware>();
        app.UseMiddleware<AddCorrelationIdToResponseMiddleware>();
        app.UseMiddleware<SerilogMiddleware>();
        app.UseMiddleware<JwtMiddleware>();

        #endregion Middlewares

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); }
        );

        #region Consul

        app.UseConsul();

        #endregion Consul
    }

    private static void ApplyMigrations(IHost host)
    {
        using var scope = Container.BeginLifetimeScope();

        try
        {
            var context = scope.Resolve<ApplicationDbContext>();
            // Your logic here...
            context.Database.Migrate();
            var uow = scope.Resolve<IMyUnitOfWork>();
            IdentitySeed.SeedUsersAsync(context, uow).Wait();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred while migrating or initializing the database.");
        }
    }

    private static IConfiguration GetConfiguration()
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{environmentName}.json", true, true)
            .AddEnvironmentVariables();

        return configurationBuilder.Build();
    }

    private static ILogger CreateSerilogLogger(IConfiguration configuration)
    {
        var seqServerUrl = configuration["Serilog:SeqServerUrl"];
        var logstashUrl = configuration["Serilog:LogstashUrl"];
        var sqlite = configuration["ConnectionStrings:Sqlite"];
        var mssql = configuration["ConnectionStrings:MSSql"];
        var appInstanceName = configuration["InstanceName"];
        var environment = configuration["Environment"];

        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty("Application", appInstanceName)
            .Enrich.WithProperty("Environment", environment)
            .Enrich.WithAssemblyName()
            .Enrich.WithAssemblyVersion()
            .Enrich.WithEnvironmentUserName() // environments are tricky when using a windows service
            //.Enrich.WithExceptionData()
            //.Enrich.WithExceptionStackTraceHash()
            .Enrich.WithMemoryUsage()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .Enrich.FromLogContext()
            .Enrich.WithProcessName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithEnvironment(environment)
            .Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached)
            .WriteTo.Console(theme: AnsiConsoleTheme.Code,
                outputTemplate:
                "{Timestamp:HH:mm} [{Level}] [{Address}] {Site}: {Message} || CommandType: [{Command_Type}], CommandId: [{Command_Id}], Application: [{Application}], Machine: [{MachineName}], User: [{EnvironmentUserName}], CorrelationId: [{CorrelationId}], DebuggerAttached: [{DebuggerAttached}] {NewLine}")
            .WriteTo.File(appInstanceName + ".log", rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: null)
            .CreateLogger();
    }
}