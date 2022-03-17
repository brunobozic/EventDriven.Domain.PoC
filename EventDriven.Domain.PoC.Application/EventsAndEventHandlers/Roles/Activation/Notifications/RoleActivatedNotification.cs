using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;
using System;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.Activation.Notifications
{
    public class RoleActivatedNotification : IntegrationEventBase<RoleActivatedDomainEvent>
    {
        public RoleActivatedNotification(RoleActivatedDomainEvent integrationEvent) : base(integrationEvent)
        {
            RoleId = integrationEvent.RoleId;
            RoleDescription = integrationEvent.Description;
            RoleName = integrationEvent.Name;
            DateActivated = integrationEvent.DateActivated;
            ActivatorEmail = integrationEvent.ActivatorEmail;
            CreatorUsername = integrationEvent.ActivatorUsername;
            CreatorId = integrationEvent.ActivatorId;
        }

        [JsonConstructor]
        public RoleActivatedNotification(long roleId) : base(null)
        {
            RoleId = roleId;
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
}