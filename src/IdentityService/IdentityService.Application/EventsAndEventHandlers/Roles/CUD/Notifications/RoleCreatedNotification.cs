using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Roles.CUD.Notifications;

public class RoleCreatedNotification : IntegrationEventBase<RoleCreatedDomainEvent>
{
    public string CreatorEmail;
    public Guid? CreatorId;
    public string CreatorUsername;
    public DateTimeOffset? DateCreated;
    public string Description;
    public string Name;
    public Guid RoleId;

    public RoleCreatedNotification(RoleCreatedDomainEvent integrationEvent) : base(integrationEvent)
    {
        RoleId = integrationEvent.RoleId;
        Description = integrationEvent.Description;
        Name = integrationEvent.Name;
        DateCreated = integrationEvent.DateCreated;
        RoleId = integrationEvent.RoleId;
        CreatorEmail = integrationEvent.CreatorEmail;
        CreatorUsername = integrationEvent.CreatorUsername;
        CreatorId = integrationEvent.CreatorId;
    }


}