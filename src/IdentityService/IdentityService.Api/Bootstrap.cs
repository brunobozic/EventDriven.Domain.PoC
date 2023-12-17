using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using Confluent.Kafka;
using Framework.Kafka.Core;
using Framework.Kafka.Core.Contracts;
using IdentityService.Api.Controllers;
using IdentityService.Api.QuartzJobs;
using IdentityService.Application.CQRSBoilerplate.Command;
using IdentityService.Application.CQRSBoilerplate.Command.Handlers;
using IdentityService.Application.CQRSBoilerplate.DomainEventDispatchers;
using IdentityService.Application.CQRSBoilerplate.UnitOfWorkImplementations;
using IdentityService.Application.DomainServices.UserServices;
using IdentityService.Application.EventsAndEventHandlers.Users.CUD.Notifications;
using IdentityService.Application.Ports.Input.Contracts;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;
using IdentityService.Data.CustomUnitOfWork;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContext;
using IdentityService.Data.DomainEventDispatching;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;
using SharedKernel.Autofac;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers;
using SharedKernel.Helpers.Configuration;
using SharedKernel.Helpers.Database;
using SharedKernel.Kafka.ConsumedMessagePersistors;
using SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;
using SharedKernel.Kafka.KafkaImplementions;
using URF.Core.Abstractions.Services;
using URF.Core.Services;

namespace IdentityService.Api;

internal static class Assemblies
{
    public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
}

/// <summary>
/// </summary>
public class Bootstrap
{
    /// <summary>
    /// </summary>
    public static IContainer Container { get; private set; }

    /// <summary>
    ///     Takes your connection string and your collection of services and cross-wires everything.
    ///     Returns a fully populated and configured AutoFac container instance.
    /// </summary>
    /// <param name="connStr"></param>
    /// <param name="services"></param>
    /// <param name="environment"></param>
    /// <returns></returns>
    public static IContainer BuildContainer(string connStr, IServiceCollection services,
        IWebHostEnvironment environment)
    {
        // create a builder
        var containerBuilder = new ContainerBuilder();
        // detect assembly name
        var executingAssembly = Assembly.GetExecutingAssembly();
        var path = Assembly.GetEntryAssembly().Location;
        // detect process module
        var processModule = Process.GetCurrentProcess().MainModule;
        // build a .net core native service provider so we can later cross-wire it with AutoFac
        var builtServiceProvider = services.BuildServiceProvider();

        IExecutionContextAccessor executionContextAccessor =
            new ExecutionContextAccessor(builtServiceProvider.GetService<IHttpContextAccessor>());

        // cross-wire services that are detected within the native .net core provider with AutoFac
        containerBuilder.Populate(services);

        containerBuilder.RegisterInstance(executionContextAccessor);

        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

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

        // => => => => => => Register your stuff here!

        containerBuilder.RegisterType(typeof(Mediator)).As(typeof(IMediator)).InstancePerLifetimeScope();

        containerBuilder.RegisterInstance(Log.Logger)
            .As<ILogger>()
            .SingleInstance();

        containerBuilder.RegisterType(typeof(UserService)).As(typeof(IUserService)).InstancePerLifetimeScope();
        containerBuilder.RegisterType(typeof(UserController)).As(typeof(IUserController))
            .InstancePerLifetimeScope();
        containerBuilder.RegisterType<UserController>().PropertiesAutowired();

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

        //container.Register(componentContext =>
        //{
        //    var serviceProvider = componentContext.Resolve<IServiceProvider>();
        //    var configuration = componentContext.Resolve<IConfiguration>();
        //    var dbContextOptions = new DbContextOptions<ApplicationDbContext>(new Dictionary<Type, IDbContextOptionsExtension>());
        //    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>(dbContextOptions)
        //        .UseApplicationServiceProvider(serviceProvider)
        //        .UseSqlite(configuration.GetConnectionString("SQLite"));

        //    return optionsBuilder.Options;
        //})
        //.As<DbContextOptions<ApplicationDbContext>>()
        //.InstancePerLifetimeScope();

        ////container.Register(context => context.Resolve<DbContextOptions<ApplicationDbContext>>())
        ////    .As<DbContextOptions<ApplicationDbContext>>()
        ////    .InstancePerLifetimeScope();

        ////container.Register(context => context.Resolve<DbContextOptions<ApplicationDbContext>>())
        ////    .As<DbContextOptions>()
        ////    .InstancePerLifetimeScope();

        //container.RegisterType<ApplicationDbContext>()
        //    .AsSelf().As<DbContext>()
        //    .InstancePerLifetimeScope();

        containerBuilder.Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder();
                dbContextOptionsBuilder.UseSqlite(connStr,
                    x => x.MigrationsAssembly("IdentityService.Data"));
                dbContextOptionsBuilder
                    .ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
                dbContextOptionsBuilder.EnableSensitiveDataLogging(); // only for local development
                dbContextOptionsBuilder.EnableDetailedErrors(); // only for local development

                return new ApplicationDbContext(dbContextOptionsBuilder.Options);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope(); // within one lifetime, will keep getting the same instance, different lifetimes will resolve a different instance!

        #endregion Entity Framework

        #region MediatR

        //container.RegisterAssemblyTypes(typeof(ApplicationUserCreatedNotification).GetTypeInfo().Assembly)
        //    .AsClosedTypesOf(typeof(IIntegrationEvent<>)).InstancePerLifetimeScope()
        //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

        //container.RegisterType<ApplicationUserCreatedEventHandler>()
        //       .Named<INotificationHandler<ApplicationUserCreatedNotification>>("handler")
        //       .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

        //container.RegisterDecorator<INotificationHandler<ApplicationUserCreatedNotification>>(
        //        (c, inner) => new DomainEventsDispatcherNotificationHandlerDecorator<ApplicationUserCreatedNotification>(c.Resolve<IDomainEventsDispatcher>(), inner),
        //        fromKey: "handler")
        //    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

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
                .FindConstructorsWith(new AllConstructorFinder())
                .AsImplementedInterfaces();

        containerBuilder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        containerBuilder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

        //containerBuilder.Register<ServiceFactory>(ctx =>
        //{
        //    var c = ctx.Resolve<IComponentContext>();
        //    return t => c.Resolve(t);
        //});

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

        containerBuilder.RegisterAssemblyTypes(Assemblies.Application)
            .AsClosedTypesOf(typeof(IDomainEventNotification<>))
            .InstancePerDependency()
            .FindConstructorsWith(new AllConstructorFinder());

        #endregion MediatR

        #region Quartz

        // This section defines the *VERY IMPORTANT* job runners without which the app, such as it is (event driven) will not function
        // The code here basically runs background jobs that pick up internal commands and integration events from db tables and execute them
        // Therefore without these jobs, the db tables will keep filling up with rows (jobs) that will never get handled

        containerBuilder.RegisterAssemblyTypes(executingAssembly).Where(x => typeof(IJob).IsAssignableFrom(x))
            .InstancePerDependency();

        #endregion Quartz

        // finally build the container itself
        var builtContainer = containerBuilder.Build();

        Container = builtContainer;

        ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(builtContainer));

        // populate the composition root container so we can use this code later on to define scope (BeginLifetimeScope)
        CompositionRoot.SetContainer(builtContainer);

        return builtContainer;
    }

    private static string GetBasePath()
    {
        using var processModule = Process.GetCurrentProcess().MainModule;
        return Path.GetDirectoryName(processModule?.FileName);
    }
}