using System;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;

public class UserUpdatedAddressDomainEvent : DomainEventBase
{
    public Guid UserId { get; set; }
    public long AddressId { get; set; }
}