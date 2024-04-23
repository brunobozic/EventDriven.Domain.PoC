using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;

public class UserUpdatedPrimaryPhoneDomainEvent : DomainEventBase
{
    public Guid UserId { get; set; }
    public long Id { get; set; }
}