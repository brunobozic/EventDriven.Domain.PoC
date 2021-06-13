using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD
{
    public class UserUpdatedMailAddressDomainEvent : DomainEventBase
    {
        public UserId UserId { get; set; }
        public long Id { get; set; }
    }
}