using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.Activation.Notifications;

public class RoleActivatedNotification : IntegrationEventBase<RoleActivatedDomainEvent>
{
    public RoleActivatedNotification(RoleActivatedDomainEvent integrationEvent, Guid id) : base(integrationEvent)
    {
        RoleId = integrationEvent.RoleId;
        RoleDescription = integrationEvent.Description;
        RoleName = integrationEvent.Name;
        DateActivated = integrationEvent.DateActivated;
        ActivatorEmail = integrationEvent.ActivatorEmail;
        CreatorUsername = integrationEvent.ActivatorUsername;
        CreatorId = integrationEvent.ActivatorId;
    }


    public string ActivatorEmail { get; set; }

    public DateTimeOffset DateActivated { get; set; }

    public string RoleName { get; set; }

    public string RoleDescription { get; set; }

    public string Description { get; set; }

    public string Name { get; set; }

    public DateTimeOffset? DateCreated { get; set; }

    public string CreatorEmail { get; set; }

    public string CreatorUsername { get; set; }

    public long CreatorId { get; set; }

    public long RoleId { get; set; }
}