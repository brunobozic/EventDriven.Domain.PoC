using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.CUD.Notifications;

public class RoleDeletedNotification : IntegrationEventBase<RoleDeletedDomainEvent>
{
    public RoleDeletedNotification(RoleDeletedDomainEvent integrationEvent) : base(integrationEvent)
    {
        RoleId = integrationEvent.RoleId;
        DeletedByUserId = integrationEvent.DeletedByUserId;
        DeletionReason = integrationEvent.DeletionReason;
        RoleName = integrationEvent.RoleName;
    }

    [JsonConstructor]
    public RoleDeletedNotification(long roleId) : base(null)
    {
        RoleId = roleId;
    }

    public string RoleName { get; set; }

    public string DeletionReason { get; set; }

    public long DeletedByUserId { get; set; }

    public long RoleId { get; set; }
}