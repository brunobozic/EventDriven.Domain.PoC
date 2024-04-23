using AspNetCoreRateLimit;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using EventDriven.Domain.PoC.Api.Rest.Helpers.ExceptionFilters;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using IdentityService.Api.Controllers;
using IdentityService.Api.Extensions;
using IdentityService.Api.Filters;
using IdentityService.Api.Middleware;
using IdentityService.Api.QuartzJobs;
using IdentityService.Api.SwaggerOverrides;
using IdentityService.Application.AutomapperMaps;
using IdentityService.Application.CommandsAndHandlers.Users.CUD;
using IdentityService.Application.CQRSBoilerplate.Command;
using IdentityService.Application.CQRSBoilerplate.DomainEventDispatchers;
using IdentityService.Application.DomainServices.EmailServices;
using IdentityService.Application.DomainServices.UserServices;
using IdentityService.Data;
using IdentityService.Data.CustomUnitOfWork;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContexts;
using IdentityService.Data.DomainEventDispatching;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;
using SharedKernel.DomainContracts;
using SharedKernel.DomainImplementations.BaseClasses;
using SharedKernel.Extensions;
using SharedKernel.Helpers.Configuration;
using SharedKernel.Helpers.Database;
using SharedKernel.Helpers.EmailSender;
using SharedKernel.Helpers.Quartz;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;
using ILogger = Serilog.ILogger;

namespace IdentityService.Api;

public class Startup
{
    public static readonly string Namespace = typeof(Program).Namespace;
    public static readonly string AppName = Namespace;

    public IWebHostEnvironment Env { get; set; }

    public IConfiguration Configuration { get; }
    // Constructor that takes IConfiguration and IWebHostEnvironment
    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        Env = env;
    }
    private MapperConfiguration MapperConfiguration { get; set; }
    public ILifetimeScope LIFETIMESCOPE { get; private set; }

    public void ConfigureServices(IServiceCollection services)
    {
        var connStr = "";

        if (Env.IsEnvironment("DockerDevelopment"))
            connStr = Configuration.GetConnectionString("MSSqlDocker");
        else if (Env.IsEnvironment("Docker"))
            connStr = Configuration.GetConnectionString("Docker");
        else
            connStr = Configuration.GetConnectionString("MSSql");

        connStr = Configuration.GetConnectionString("Sqlite");

        services.AddOptions();
        services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));


        var assembly = Assembly.GetExecutingAssembly(); // Change this to your assembly

        //Register services dynamically
        services.RegisterServicesWithAttributes(assembly);

        #region MVC wireup

        services.AddMvc(opt =>
            {
                // opt.Filters.Add(typeof(ValidateFilterAttribute));
            })
            .AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssembly(
                    Assembly.Load(
                        "IdentityService.Application")); // the assembly that houses the implemented validators
                // fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false; // dont run MVC validators after having run the fluent ones
                fv.ImplicitlyValidateChildProperties =
                    true; // fall through and validate all child elements and their child elements
            })
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
            .AddControllersAsServices(); // as the name implies, this makes all controllers become registered with IoC just as any other class would be (by default, they are not)

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
        // Add FV
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        // Add FV Rules to swagger
        services.AddFluentValidationRulesToSwagger();
        // [Optional] Configure generation options for your needs. Also can be done with services.Configure<SchemaGenerationOptions>
        // services.AddFluentValidationRulesToSwagger(options =>
        // {
        //     options.SetNotNullableIfMinLengthGreaterThenZero = true;
        //     options.UseAllOffForMultipleRules = true;
        // });

        #endregion Swagger

        services.Configure<MyConfigurationValues>(Configuration.GetSection("MyConfigurationValues"));
        services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<MyConfigurationValues>>().Value);

        #region DB, Entities, UOW, repos

        services.AddDbContextPool<ApplicationDbContext>((serviceProvider, options) =>
        {
            // Configuring the DbContext based on the application's running environment
            if (Configuration.GetValue<bool>("UseInMemory"))
            {
                options.UseInMemoryDatabase(nameof(ApplicationDbContext))
                       .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));

                if (Env.IsDevelopment() || Env.IsEnvironment("DockerDevelopment"))
                {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                }
            }
            else if (Env.IsEnvironment("Test"))
            {
                var connStr = Configuration.GetConnectionString("SqliteTest"); // Ensure you have this in your config
                options.UseSqlite(connStr, x => x.MigrationsAssembly("IdentityService.Data"));
            }
            else
            {
                var connStr = Configuration.GetConnectionString("Sqlite");
                options.UseSqlite(connStr, x => x.MigrationsAssembly("IdentityService.Data"));
            }
        }, poolSize: 128); // Pool size is adjustable based on your application's needs

        // Transient registration of ApplicationDbContext to ensure it can be injected specifically if needed
        services.AddScoped<ApplicationDbContext>();

        // Additional repository and unit of work registrations
        services.RegisterRepositories();
        services.AddSingleton<DbContext>(provider => provider.GetService<ApplicationDbContext>());

        #endregion DB, Entities, UOW, repos

        #region Authentication

        var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
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


        #region Rate limiting

        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429;
            options.RealIpHeader = "X-Real-IP";
            options.ClientIdHeader = "X-ClientId";
            options.GeneralRules = new System.Collections.Generic.List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "20s",
                    Limit = 3
                }
            };
        });

        #endregion Rate limiting


        #region Service Registration


        services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddTransient<IEmailService, GmailService>();
        // services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

        services.Configure<SmtpOptions>(Configuration.GetSection(nameof(SmtpOptions)));
        services.Configure<MailOptions>(Configuration.GetSection(nameof(MailOptions)));
        services.Configure<JwtIssuerOptions>(Configuration.GetSection(nameof(JwtIssuerOptions)));

        services.AddTransient<IUserService, UserService>();

        #region Mediatr Registration

        // Assuming IdentityService.Application assembly contains your handlers
        var mediatrAssembly = typeof(RegisterUserCommandHandler).Assembly;



        #endregion Mediatr Registration

        services.AddScoped<IMyUnitOfWork, MyUnitOfWork>(); // also in Bootstrap.cs
        services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>(); // also in Bootstrap.cs
        services.AddScoped<ICommandsScheduler, CommandsScheduler>(); // also in Bootstrap.cs
        services.AddScoped<ISqlConnectionFactory>(provider => new SqlConnectionFactory(connStr)); // also in Bootstrap.cs
        services.AddScoped<DbContextOptions, DbContextOptions<ApplicationDbContext>>();
        services.AddInMemoryRateLimiting();
        #region AD

        // Uncomment this when the AD comes into play!
        //services.AddScoped<IUserProvider, AdUserProvider>();

        #endregion AD

        #endregion Service Registration

        #region Consul

        services.AddConsulConfig(Configuration);

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

        //adding healthchecks UI
        services.AddHealthChecksUI(opt =>
        {
            opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
            opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
            opt.SetApiMaxActiveRequests(1); //api requests concurrency
            opt.AddHealthCheckEndpoint("api", "/health"); //map health check api
        }).AddSqliteStorage(connStr);

        #endregion HealthCheck

        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
        });

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

        // ================================================================================================
        // ================================================================================================
        // ==================================== AutoMapper ===============================================
        // ================================================================================================
        // ================================================================================================
        MapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new DomainToViewModelMappingProfile());
            cfg.AddProfile(new ViewModelToDomainMappingProfile());
        });

        services.AddSingleton(sp => MapperConfiguration.CreateMapper());

        var AMconfig = new MapperConfiguration(cfg => { cfg.AddMaps("IdentityService.Application"); });

        var mapper = AMconfig.CreateMapper();
        //config.AssertConfigurationIsValid();
        services.AddSingleton(mapper);

        // ================================================================================================
        // ================================================================================================
        // ==================================== / AutoMapper ==============================================
        // ================================================================================================
        // ================================================================================================


        // ================================================================================================
        // ================================================================================================
        // ========================================   OTEL   ==============================================
        // ================================================================================================
        // ================================================================================================
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
        // ================================================================================================
        // ================================================================================================
        // ========================================   /OTEL   =============================================
        // ================================================================================================
        // ================================================================================================
        // Add and configure MediatR

        var mediatrAssembly2 = typeof(DomainEventsDispatcherNotificationHandlerDecorator<>).Assembly;
        var mediatrAssembly3 = typeof(DomainEventBase).Assembly;
        var mediatrAssembly4 = typeof(UserController).Assembly;

        var ass = new Assembly[3];
        ass[0] = mediatrAssembly2;
        ass[1] = mediatrAssembly3;
        ass[2] = mediatrAssembly4;
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(ass);
        });

        // ================================================================================================
        // ================================================================================================
        // ======================================== AutoFac ===============================================
        // ================================================================================================
        // ================================================================================================

        var builtContainer = Bootstrap.BuildContainer(connStr, services, Env);
        var serviceProvider = new AutofacServiceProvider(builtContainer);

        // ================================================================================================
        // ================================================================================================
        // ======================================== / AutoFac =============================================
        // ================================================================================================
        // ================================================================================================

        // ================================================================================================
        // ================================================================================================
        // ========================================     Quartz       =====================================
        // ================================================================================================
        // ================================================================================================


        var schedulerFactory = new StdSchedulerFactory();
        var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

        scheduler.JobFactory = new JobFactory(builtContainer);

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
        //=======================================
        //=====   Kafka polling consumer   ======
        //=======================================
        var processKafkaPollJob = JobBuilder.Create<KafkaPollJob>().Build();

        var triggerKafkaPollJob =
            TriggerBuilder
                .Create()
                .StartNow()
                .WithCronSchedule("0/15 * * ? * *")
                .Build();

        scheduler.ScheduleJob(processKafkaPollJob, triggerKafkaPollJob).GetAwaiter().GetResult();
        // ================================================================================================
        // ================================================================================================
        // ======================================      / Quartz       =====================================
        // ================================================================================================
        // ================================================================================================

        serviceProvider = new AutofacServiceProvider(builtContainer);

        return;
    }

    public void ConfigureContainer(ContainerBuilder containerBuilder)
    {

    }

    public class MediatRServiceConfiguration
    {
        public List<Assembly> AssembliesToScan { get; } = new List<Assembly>();

        public void AddAssemblyToScan(Assembly assembly)
        {
            AssembliesToScan.Add(assembly);
        }

        public List<Type> PipelineBehaviors { get; } = new List<Type>();

        public void AddPipelineBehavior(Type behaviorType)
        {
            PipelineBehaviors.Add(behaviorType);
        }

        // Additional configuration methods can be added as needed
    }

    public void Configure(
        IApplicationBuilder app
        , IWebHostEnvironment env
        , ApplicationDbContext myDbContext
        , IOptions<ServiceDisvoveryOptions> serviceOptions
        , IApplicationLifetime appLife
    )
    {
        LIFETIMESCOPE = app.ApplicationServices.GetAutofacRoot();

        InitializeModules(LIFETIMESCOPE);


        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        _ = DbInitializer.InitializeAsync(myDbContext);

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

        app.UseHealthChecks("/hc", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

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

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                       ForwardedHeaders.XForwardedProto
        });

        #region Global exception handler

        var exceptionHandlerOptions = new ExceptionHandlerOptions
        {
            ExceptionHandler = async context =>
            {
                if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
                    await ExceptionHandler();
                return;

                async Task ExceptionHandler()
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var statusCode = exceptionHandlerFeature!.Error switch
                    {
                        ApplicationSpecificException => StatusCodes.Status418ImATeapot,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    context.Response.StatusCode = statusCode;

                    var problemDetails = new ProblemDetails
                    {
                        Title = "A problem has happened",
                        Detail = exceptionHandlerFeature.Error.Message,
                        Status = statusCode
                    };

                    if (env.IsDevelopment())
                    {
                        problemDetails.Title = exceptionHandlerFeature.Error.GetType().ToString();
                        problemDetails.Extensions["exception"] = new
                        {
                            Details = exceptionHandlerFeature.Error.ToString(),
                            context.Request.Headers,
                            Path = context.Request.Path.ToString(),
                            Endpoint = exceptionHandlerFeature.Endpoint?.ToString(),
                            exceptionHandlerFeature.RouteValues
                        };
                    }

                    await problemDetailsService.WriteAsync(new ProblemDetailsContext
                    {
                        HttpContext = context,
                        AdditionalMetadata = exceptionHandlerFeature.Endpoint?.Metadata,
                        ProblemDetails = problemDetails
                    });
                }
            }
        };

        app.UseExceptionHandler(exceptionHandlerOptions);

        #endregion Global exception handler
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
        app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //adding endpoint of health check for the health check ui in UI format
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                //map healthcheck ui endpoing - default is /healthchecks-ui/
                endpoints.MapHealthChecksUI();
            }
        );

        #region Consul

        // app.UseConsul(Configuration);

        #endregion Consul
    }

    private void InitializeModules(ILifetimeScope lIFETIMESCOPE)
    {

    }

    private ILogger ConfigureLogger(IConfiguration configuration)
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
            .Enrich.WithExceptionData()
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
              .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
              {
                  AutoRegisterTemplate = true,
                  AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                  IndexFormat = "IdentityProvider-logs-{0:yyyy.MM}"
              })
            .WriteTo.File(appInstanceName + ".log", rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: null)
            .CreateLogger();
    }
}