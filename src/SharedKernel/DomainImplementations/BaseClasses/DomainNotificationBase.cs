using SharedKernel.DomainContracts;
using System;
using System.Text.Json.Serialization;

namespace SharedKernel.DomainImplementations.BaseClasses;

public class DomainNotificationBase<T> : IDomainEventNotification<T> where T : IDomainEvent
{
    public DomainNotificationBase(T integrationEvent, Guid id)
    {
        Id = Guid.NewGuid();
        EventType = EventTypeEnum.Undefined;
        DomainEvent = integrationEvent;
    }

    public EventTypeEnum EventType { get; set; }

    public Guid Id { get; }
    [JsonIgnore] public T DomainEvent { get; }
}