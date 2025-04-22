using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;

public class EmailVerifiedDomainEvent : DomainEventBase
{
    public EmailVerifiedDomainEvent(string email, string userName, Guid userResourceId, string origin, EventTypeEnum eventType)
    {
        Email = email;
        UserName = userName;
        UserResourceId = userResourceId;
        VerifiedDate = DateTimeOffset.UtcNow;
        Origin = origin;
        EventType = eventType;
    }

    public DateTimeOffset? VerifiedDate { get; }
    public string Origin { get; set; }
    public EventTypeEnum EventType { get; set; }
    public string Email { get; }
    public Guid UserResourceId { get; }
    public string UserName { get; }
}