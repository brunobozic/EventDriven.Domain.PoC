using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.Notifications;

public class RoleAssignedToUserNotification : DomainNotificationBase<RoleAssignedToUserDomainEvent>
{
    public DateTimeOffset DateAssigned;
    public DateTimeOffset? RoleActiveTo;
    public string RoleGiverEmail;
    public Guid RoleGiverId;
    public string RoleGiverUsername;
    public long RoleId;
    public string RoleName;
    public string UserEmail;
    public Guid UserId;
    public string UserName;

    public RoleAssignedToUserNotification(RoleAssignedToUserDomainEvent integrationEvent, Guid id) : base(integrationEvent, id)
    {
        UserId = integrationEvent.UserId;
        UserName = integrationEvent.UserName;
        UserEmail = integrationEvent.Email;
        RoleName = integrationEvent.Name;
        RoleId = integrationEvent.RoleId;
        RoleActiveTo = integrationEvent.ActiveTo;
        DateAssigned = integrationEvent.DateAssigned;
        RoleGiverEmail = integrationEvent.RoleGiverEmail;
        RoleGiverUsername = integrationEvent.RoleGiverUsername;
        RoleGiverId = integrationEvent.RoleGiverId;
    }



}