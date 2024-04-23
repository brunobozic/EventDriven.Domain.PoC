using MediatR;
using System;

namespace SharedKernel.DomainImplementations.BaseClasses;


public interface IDomainEventNotification<out TEventType> : IDomainEventNotification
{
    TEventType DomainEvent { get; }
}

public interface IDomainEventNotification : INotification
{
    Guid Id { get; }
}

