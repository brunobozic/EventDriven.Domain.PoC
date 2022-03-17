using Autofac;
using EventDriven.Domain.PoC.Application.CQRSBoilerplate.Command;
using EventDriven.Domain.PoC.Application.CQRSBoilerplate.Command.Handlers;
using EventDriven.Domain.PoC.Application.CQRSBoilerplate.DomainEventDispatchers;
using EventDriven.Domain.PoC.Application.CQRSBoilerplate.UnitOfWorkImplementations;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.Repository.EF.DomainEventDispatching;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using MediatR;
using System.Reflection;
using Module = Autofac.Module;

namespace EventDriven.Domain.PoC.Application.AutofacModules
{
    public class ProcessingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IntegrationEventDispatcher>()
                .As<IDomainEventsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(UserCreatedDomainEvent).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEvent<>)).InstancePerDependency();

            builder.RegisterGenericDecorator(
                typeof(DomainEventsDispatcherNotificationHandlerDecorator<>),
                typeof(INotificationHandler<>));

            builder.RegisterGenericDecorator(
                typeof(UnitOfWorkCommandHandlerDecorator<>),
                typeof(ICommandHandler<>));

            builder.RegisterGenericDecorator(
                typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
                typeof(ICommandHandler<,>));

            builder.RegisterType<CommandsDispatcher>()
                .As<ICommandsDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CommandsScheduler>()
                .As<ICommandsScheduler>()
                .InstancePerLifetimeScope();

            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerDecorator<>),
                typeof(ICommandHandler<>));

            builder.RegisterGenericDecorator(
                typeof(LoggingCommandHandlerWithResultDecorator<,>),
                typeof(ICommandHandler<,>));
        }
    }
}