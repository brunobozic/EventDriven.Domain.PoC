﻿using System;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

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

    [JsonConstructor]
    public RoleCreatedNotification(Guid roleId) : base(null)
    {
        RoleId = roleId;
    }
}