using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.Notifications;

public class RoleRemovedFromUserNotification : DomainNotificationBase<RoleRemovedFromUserDomainEvent>
{
    public DateTimeOffset? DateRemoved;
    public string RemoverEmail;
    public Guid RemoverUserId;
    public string RemoverUsername;
    public long RoleId;
    public string RoleName;
    public string UserEmail;
    public Guid UserId;
    public string UserName;

    public RoleRemovedFromUserNotification(RoleRemovedFromUserDomainEvent integrationEvent, Guid id) : base(integrationEvent, id)
    {
        RoleId = integrationEvent.RoleId;
        RoleName = integrationEvent.Name;
        DateRemoved = integrationEvent.DateRemoved;
        UserId = integrationEvent.UserId;
        UserName = integrationEvent.UserName;
        UserEmail = integrationEvent.UserEmail;
        RemoverEmail = integrationEvent.RemoverEmail;
        RemoverUsername = integrationEvent.RemoverUsername;
        RemoverUserId = integrationEvent.RemoverUserId;
    }


}