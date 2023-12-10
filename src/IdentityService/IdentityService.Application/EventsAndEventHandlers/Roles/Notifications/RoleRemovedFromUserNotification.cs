using System;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.Notifications;

public class RoleRemovedFromUserNotification : IntegrationEventBase<RoleRemovedFromUserDomainEvent>
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

    public RoleRemovedFromUserNotification(RoleRemovedFromUserDomainEvent integrationEvent) : base(integrationEvent)
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

    [JsonConstructor]
    public RoleRemovedFromUserNotification(Guid userId, long roleId) : base(null)
    {
        UserId = userId;
        RoleId = roleId;
    }
}