//using System.Reflection;
//using Autofac;
//using IdentityService.Application.CQRSBoilerplate.Command;
//using IdentityService.Application.CQRSBoilerplate.Command.Handlers;
//using IdentityService.Application.CQRSBoilerplate.DomainEventDispatchers;
//using IdentityService.Application.CQRSBoilerplate.UnitOfWorkImplementations;
//using IdentityService.Data.DomainEventDispatching;
//using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
//using MediatR;
//using SharedKernel.DomainContracts;
//using Module = Autofac.Module;

//namespace IdentityService.Application.AutofacModules;

//public class ProcessingModule : Module
//{
//    protected override void Load(ContainerBuilder builder)
//    {
//        builder.RegisterType<IntegrationEventDispatcher>()
//            .As<IDomainEventsDispatcher>()
//            .InstancePerLifetimeScope();

//        builder.RegisterAssemblyTypes(typeof(UserCreatedDomainEvent).GetTypeInfo().Assembly)
//            .AsClosedTypesOf(typeof(IIntegrationEvent<>)).InstancePerDependency();

//        builder.RegisterGenericDecorator(
//            typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
//            typeof(INotificationHandler<>));

//        builder.RegisterGenericDecorator(
//            typeof(UnitOfWorkCommandHandlerDecorator<>),
//            typeof(ICommandHandler<>));

//        builder.RegisterGenericDecorator(
//            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
//            typeof(ICommandHandler<,>));

//        builder.RegisterType<CommandsDispatcher>()
//            .As<ICommandsDispatcher>()
//            .InstancePerLifetimeScope();

//        builder.RegisterType<CommandsScheduler>()
//            .As<ICommandsScheduler>()
//            .InstancePerLifetimeScope();

//        builder.RegisterGenericDecorator(
//            typeof(LoggingCommandHandlerDecorator<>),
//            typeof(ICommandHandler<>));

//        builder.RegisterGenericDecorator(
//            typeof(LoggingCommandHandlerWithResultDecorator<,>),
//            typeof(ICommandHandler<,>));
//    }
//}