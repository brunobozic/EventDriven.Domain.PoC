using SharedKernel.DomainContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks; 

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