using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD
{
    public class UserUpdatedAddressDomainEvent : DomainEventBase
    {
        public Guid UserId { get; set; }
        public long AddressId { get; set; }
    }
}