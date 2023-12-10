using System;
using MediatR;

namespace SharedKernel.DomainContracts;

public interface IIntegrationEvent<out TEventType> : IIntegrationEventNotification
{
    TEventType IntegrationEvent { get; }
}

public interface IIntegrationEventNotification : INotification
{
    Guid Id { get; }
}