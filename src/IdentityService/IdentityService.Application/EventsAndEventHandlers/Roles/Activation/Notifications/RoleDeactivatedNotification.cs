using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.Activation.Notifications;

public class RoleDeactivatedNotification : IntegrationEventBase<RoleDeactivatedDomainEvent>
{
    public RoleDeactivatedNotification(RoleDeactivatedDomainEvent integrationEvent, Guid id) : base(integrationEvent)
    {
        RoleId = integrationEvent.RoleId;
        RoleDescription = integrationEvent.Description;
        RoleName = integrationEvent.Name;
        DateDeactivated = integrationEvent.DateDeactivated;
        DeactivatorEmail = integrationEvent.DeactivatorEmail;
        DeactivatorUsername = integrationEvent.DeactivatorUsername;
        DeactivatorId = integrationEvent.DeactivatedById;
    }


    public string DeactivatorUsername { get; set; }
    public Guid DeactivatorId { get; set; }
    public string DeactivatorEmail { get; set; }

    public DateTimeOffset DateDeactivated { get; set; }

    public string RoleName { get; set; }

    public string RoleDescription { get; set; }

    public string Description { get; set; }

    public string Name { get; set; }
    public long RoleId { get; set; }
}