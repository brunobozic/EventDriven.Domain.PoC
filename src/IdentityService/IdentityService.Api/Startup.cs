﻿using Autofac.Extensions.DependencyInjection;
using AutoMapper;

using EventDriven.Domain.PoC.Api.Rest.Helpers.ExceptionFilters;

using FluentValidation;
using FluentValidation.AspNetCore;

using IdentityService.Api.Extensions;
using IdentityService.Api.Filters;
using IdentityService.Api.Middleware;
using IdentityService.Api.QuartzJobs;
using IdentityService.Api.SwaggerOverrides;
using IdentityService.Application.AutomapperMaps;
using IdentityService.Application.DomainServices.EmailServices;
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
using Serilog.Sinks.SystemConsole.Themes;
using SharedKernel.Helpers.Configuration;
using SharedKernel.Helpers.EmailSender;
using SharedKernel.Helpers.Quartz;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using static SharedKernel.Helpers.Startup;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;
using ILogger = Serilog.ILogger;

namespace EventDriven.Domain.PoC.Api.Rest
{
#pragma warning disable 1591

    public class Startup
#pragma warning restore 1591
    {
#pragma warning disable 1591
        public static readonly string Namespace = typeof(Program).Namespace;
#pragma warning restore 1591
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly string AppName = Namespace;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable 1591

        public Startup(
#pragma warning restore 1591
            IConfiguration configuration
            , ILoggerFactory loggerFactory
            , IWebHostEnvironment env
        )
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            Env = env;
            Logger = ConfigureLogger(configuration);
            LoggerFactory.AddSerilog(Logger);
            Logger.Information("Logger configured");
        }

#pragma warning disable 1591
        public IWebHostEnvironment Env { get; set; }
#pragma warning restore 1591
#pragma warning disable 1591
        public ILogger Logger { get; }
#pragma warning restore 1591
#pragma warning disable 1591
        public IConfiguration Configuration { get; }
#pragma warning restore 1591
#pragma warning disable 1591
        public ILoggerFactory LoggerFactory { get; set; }
#pragma warning restore 1591
        private MapperConfiguration MapperConfiguration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
#pragma warning disable 1591

        [Obsolete]
        public IServiceProvider ConfigureServices(IServiceCollection services)
#pragma warning restore 1591
        {
            var connStr = Configuration.GetConnectionString("Sqlite");
            services.AddOptions();
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));

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
                    {securitySchema, new[] {"Bearer"}}
                };
                options.AddSecurityRequirement(securityRequirement);
                // Register the custom operation filter here
                options.OperationFilter<UserRegistrationCustomOperationFilter>();
            });
            services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

            #endregion Swagger

            services.Configure<MyConfigurationValues>(Configuration.GetSection("MyConfigurationValues"));
            services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<MyConfigurationValues>>().Value);

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

            #region Service Registration

            services.RegisterRepositories();
            services.AddTransient<IValidatorFactory, ServiceProviderValidatorFactory>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IEmailService, GmailService>();
            // services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.Configure<SmtpOptions>(Configuration.GetSection(nameof(SmtpOptions)));
            services.Configure<MailOptions>(Configuration.GetSection(nameof(MailOptions)));
            services.Configure<JwtIssuerOptions>(Configuration.GetSection(nameof(JwtIssuerOptions)));

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
            //services.AddHealthChecksUI(opt =>
            //{
            //    opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
            //    opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
            //    opt.SetApiMaxActiveRequests(1); //api requests concurrency
            //    opt.AddHealthCheckEndpoint("api", "/health"); //map health check api
            //}).AddSqliteStorage(connStr);

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
                .AddService(serviceName: "OtPrGrJa"));

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
                {
                    tracing.AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
                    });
                }
                else
                {
                    tracing.AddConsoleExporter();
                }
            });
            // ================================================================================================
            // ================================================================================================
            // ========================================   /OTEL   =============================================
            // ================================================================================================
            // ================================================================================================



            // ================================================================================================
            // ================================================================================================
            // ======================================== AutoFac ===============================================
            // ================================================================================================
            // ================================================================================================

            var builtContainer = Bootstrap.BuildContainer(connStr, services, Env);

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

            var serviceProvider = new AutofacServiceProvider(builtContainer);

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app
#pragma warning restore 1591
            , IWebHostEnvironment env
            //, ILoggerFactory loggerFactory
            //, ApplicationDbContext myDbContext
            , IOptions<ServiceDisvoveryOptions> serviceOptions
            , IApplicationLifetime appLife
        )
        {
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

            //app.UseHealthChecks("/hc", new HealthCheckOptions
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});

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

            // The idea here is to hijack all exceptions, and decide whether to show the full trace (for developers) or to show a cleaned up
            // safe, user friendly messages to end users (commonly this is done in Production)
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
                                var json = new JsonErrorResponse
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

                                Serilog.Log.Error("{0} for identity: [ {1} ]", error.Error.Message, context.User.Identity.Name);
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
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                ////adding endpoint of health check for the health check ui in UI format
                //endpoints.MapHealthChecks("/health", new HealthCheckOptions
                //{
                //    Predicate = _ => true,
                //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                //});

                ////map healthcheck ui endpoing - default is /healthchecks-ui/
                //endpoints.MapHealthChecksUI();
            }
            );

            #region Consul

            app.UseConsul();

            #endregion Consul
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
}