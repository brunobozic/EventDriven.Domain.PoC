using System;
using System.Text.Json.Serialization;
using SharedKernel.DomainContracts;

namespace SharedKernel.DomainImplementations.BaseClasses;

public class IntegrationEventBase<T> : IIntegrationEvent<T> where T : IDomainEvent
{
    public IntegrationEventBase(T integrationEvent)
    {
        Id = Guid.NewGuid();
        EventType = EventTypeEnum.Undefined;
        IntegrationEvent = integrationEvent;
    }

    public EventTypeEnum EventType { get; set; }

    public Guid Id { get; }
    [JsonIgnore] public T IntegrationEvent { get; }
}