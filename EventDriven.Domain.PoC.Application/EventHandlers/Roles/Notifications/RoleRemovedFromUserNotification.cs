using System;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Roles.Notifications
{
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
}