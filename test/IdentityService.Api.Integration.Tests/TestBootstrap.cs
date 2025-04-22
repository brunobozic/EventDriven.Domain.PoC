using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using IdentityService.Application.DomainServices.UserServices;
using IdentityService.Api.Controllers;
using IdentityService.Application.CQRSBoilerplate.Command;
using IdentityService.Application.CQRSBoilerplate.Command.Handlers;
using IdentityService.Application.CQRSBoilerplate.DomainEventDispatchers;
using IdentityService.Application.CQRSBoilerplate.OutboxCommands;
using IdentityService.Application.CQRSBoilerplate.UnitOfWorkImplementations;
using IdentityService.Data.CustomUnitOfWork;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContexts;
using IdentityService.Data.DomainEventDispatching;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.Autofac;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers.Database;
using URF.Core.Abstractions.Services;
using URF.Core.Services;
using Serilog;
using System.Runtime.InteropServices;
using System;
using Confluent.Kafka;
using Framework.Kafka.Core.Contracts;
using Framework.Kafka.Core.KafkaSettings;
using IdentityService.Api.QuartzJobs;
using Quartz;
using SharedKernel.Helpers.Configuration;
using SharedKernel.Kafka.ConsumedMessagePersistors;
using SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;
using SharedKernel.Kafka.KafkaImplementions;
using SharedKernel.Helpers;
using Framework.Kafka.Core;
using CommonServiceLocator;
using Autofac.Extras.CommonServiceLocator;
using IdentityService.Api;
using IdentityService.Application.EventsAndEventHandlers.Users.CUD.Notifications;
using IdentityService.Application.Ports.Input.Contracts;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;
using MediatR.Pipeline;

namespace IdentityService.Tests.Integration.Fixtures
{
    public static class Assemblies
    {
        public static readonly Assembly Application = typeof(InternalCommandBase).Assembly;
    }

    public static class TestBootstrap
    {
        public static void ConfigureContainer(ContainerBuilder containerBuilder, IConfiguration configuration, IHostEnvironment environment)
        {
            // Bind appsetting values to a configuration class
            var settings = new MyConfigurationValues();
            configuration.GetSection("MyConfigurationValues").Bind(settings);
            containerBuilder.RegisterInstance(settings).As<MyConfigurationValues>().SingleInstance();

            containerBuilder.RegisterType<UserActivatedDomainEvent>();

            #region Kafka

            // Register Kafka configurations and services only if not in Test environment
            if (environment.EnvironmentName != "Test")
            {
                // Kafka Consumer Config
                var kafkaConsumerConfig = new ConsumerConfig
                {
                    // Your Kafka Consumer Config settings
                };

                // Kafka Producer Config
                var kafkaProducerConfig = new ProducerConfig
                {
                    // Your Kafka Producer Config settings
                };

                // Register Kafka configurations
                containerBuilder.RegisterInstance(kafkaConsumerConfig).As<ConsumerConfig>().SingleInstance();
                containerBuilder.RegisterInstance(kafkaProducerConfig).As<ProducerConfig>().SingleInstance();

                // Register Kafka services
                containerBuilder.RegisterType<KafkaScheduledConsumer>().As<IKafkaScheduledConsumer>()
                    .WithParameter("topicName", settings.KafkaConsumerSettings.KafkaTopic)
                    .SingleInstance();

                containerBuilder.RegisterType<KafkaScheduledProducer>().As<IKafkaScheduledProducer>()
                    .WithParameter("topicName", settings.KafkaLoggingProducerSettings.KafkaTopic)
                    .SingleInstance();

                containerBuilder.RegisterType<KafkaLoggingProducer>().As<IKafkaLoggingProducer>()
                    .WithParameter("topicName", settings.KafkaLoggingProducerSettings.KafkaTopic)
                    .SingleInstance();

                containerBuilder.RegisterType<ConsumedMessagePersistor>().As<IConsumedMessagePersistor>()
                    .SingleInstance();

                containerBuilder.RegisterType<KafkaPollJobController>()
                    .As<IJobController>()
                    .UsingConstructor(typeof(MyConfigurationValues), typeof(IKafkaScheduledConsumer), typeof(IConsumedMessagePersistor));
            }

            #endregion Kafka

            #region Application Services

            containerBuilder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
            containerBuilder.RegisterInstance(Log.Logger).As<Serilog.ILogger>().SingleInstance();
            containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<UserController>().As<IUserController>().InstancePerLifetimeScope();

            #endregion Application Services

            #region Entity Framework

            containerBuilder.RegisterType<MyUnitOfWork>().As<IMyUnitOfWork>().InstancePerLifetimeScope();
            containerBuilder.RegisterGeneric(typeof(Service<>)).As(typeof(IService<>)).InstancePerLifetimeScope();

            containerBuilder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", configuration.GetConnectionString("DefaultConnection"))
                .InstancePerLifetimeScope();

            containerBuilder.RegisterType<StronglyTypedIdValueConverterSelector>()
                .As<IValueConverterSelector>()
                .InstancePerLifetimeScope();

            containerBuilder.Register(c =>
            {
                var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                dbContextOptionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"), x => x.MigrationsAssembly("IdentityService.Data"));
                dbContextOptionsBuilder.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
                dbContextOptionsBuilder.EnableSensitiveDataLogging();
                dbContextOptionsBuilder.EnableDetailedErrors();

                return new ApplicationDbContext(dbContextOptionsBuilder.Options);
            })
            .AsSelf()
            .As<DbContext>()
            .InstancePerLifetimeScope();

            #endregion Entity Framework

            #region MediatR

            containerBuilder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(INotificationHandler<>)
            };

            foreach (var mediatrOpenType in mediatrOpenTypes)
                containerBuilder
                    .RegisterAssemblyTypes(typeof(CreateUserCommand).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();

            containerBuilder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            containerBuilder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            containerBuilder.RegisterType<Api.IntegrationEventDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(typeof(UserCreatedNotification).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEvent<>))
                .InstancePerDependency();

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

            containerBuilder.RegisterType<IntegrationEventPersistor>()
                .As<IIntegrationEventPersistor>()
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

            containerBuilder.RegisterAssemblyTypes(Assemblies.Application)
                .AsClosedTypesOf(typeof(IIntegrationEvent<>))
                .InstancePerDependency()
                .FindConstructorsWith(new AllConstructorFinder());

            #endregion MediatR

            #region Quartz

            var executingAssembly = Assembly.GetExecutingAssembly();
            containerBuilder.RegisterAssemblyTypes(executingAssembly).Where(x => typeof(IJob).IsAssignableFrom(x))
                .InstancePerDependency();

            #endregion Quartz
        }
    }
}
