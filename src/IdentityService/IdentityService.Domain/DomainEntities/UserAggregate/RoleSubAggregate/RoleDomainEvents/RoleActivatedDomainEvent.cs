using System;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;

public class RoleActivatedDomainEvent : DomainEventBase
{
    public long ActivatedById;

    public string ActivationReason;
    public string ActivatorEmail;
    public long ActivatorId;
    public string ActivatorUsername;
    public DateTimeOffset DateActivated;
    public string Description;
    public string Name;
    public long RoleId;

    public RoleActivatedDomainEvent(long roleId, long activatedById, string activatedByUserEmail,
        string activatedByUserUserName, string activationReason,
        DateTimeOffset dateActivated)
    {
        RoleId = roleId;
        ActivatedById = activatedById;
        ActivationReason = activationReason;
        DateActivated = dateActivated;
        ActivatorEmail = activatedByUserEmail;
        ActivatorUsername = activatedByUserUserName;
    }
}