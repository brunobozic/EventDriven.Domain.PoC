using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;
using System;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.CUD.Notifications
{
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
}