using System;
using System.Text.Json.Serialization;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.Activation.Notifications
{
    public class RoleDeactivatedNotification : IntegrationEventBase<RoleDeactivatedDomainEvent>
    {
        public RoleDeactivatedNotification(RoleDeactivatedDomainEvent integrationEvent) : base(integrationEvent)
        {
            RoleId = integrationEvent.RoleId;
            RoleDescription = integrationEvent.Description;
            RoleName = integrationEvent.Name;
            DateDeactivated = integrationEvent.DateDeactivated;
            DeactivatorEmail = integrationEvent.DeactivatorEmail;
            DeactivatorUsername = integrationEvent.DeactivatorUsername;
            DeactivatorId = integrationEvent.DeactivatedById;
        }

        [JsonConstructor]
        public RoleDeactivatedNotification(long roleId) : base(null)
        {
            RoleId = roleId;
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
}