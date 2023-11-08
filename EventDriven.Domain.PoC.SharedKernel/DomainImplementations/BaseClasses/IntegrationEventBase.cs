using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using System;
using System.Text.Json.Serialization;

namespace EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses
{
    public class IntegrationEventBase<T> : IIntegrationEvent<T> where T : IDomainEvent
    {
        public IntegrationEventBase(T integrationEvent)
        {
            Id = Guid.NewGuid();
            IntegrationEvent = integrationEvent;
        }

        public Guid Id { get; }
        [JsonIgnore] public T IntegrationEvent { get; }
    }
}