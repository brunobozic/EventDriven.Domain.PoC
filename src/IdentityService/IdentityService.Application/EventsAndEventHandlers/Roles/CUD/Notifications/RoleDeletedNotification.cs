using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.CUD.Notifications;

public class RoleDeletedNotification : DomainNotificationBase<RoleDeletedDomainEvent>
{
    public RoleDeletedNotification(RoleDeletedDomainEvent integrationEvent, Guid id) : base(integrationEvent, id)
    {
        RoleId = integrationEvent.RoleId;
        DeletedByUserId = integrationEvent.DeletedByUserId;
        DeletionReason = integrationEvent.DeletionReason;
        RoleName = integrationEvent.RoleName;
    }


    public string RoleName { get; set; }

    public string DeletionReason { get; set; }

    public long DeletedByUserId { get; set; }

    public long RoleId { get; set; }
}