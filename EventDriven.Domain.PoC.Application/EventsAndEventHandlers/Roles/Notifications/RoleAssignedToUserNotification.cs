using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;
using System;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Roles.Notifications
{
    public class RoleAssignedToUserNotification : IntegrationEventBase<RoleAssignedToUserDomainEvent>
    {
        public DateTimeOffset DateAssigned;
        public DateTimeOffset? RoleActiveTo;
        public string RoleGiverEmail;
        public Guid RoleGiverId;
        public string RoleGiverUsername;
        public long RoleId;
        public string RoleName;
        public string UserEmail;
        public Guid UserId;
        public string UserName;

        public RoleAssignedToUserNotification(RoleAssignedToUserDomainEvent integrationEvent) : base(integrationEvent)
        {
            UserId = integrationEvent.UserId;
            UserName = integrationEvent.UserName;
            UserEmail = integrationEvent.Email;
            RoleName = integrationEvent.Name;
            RoleId = integrationEvent.RoleId;
            RoleActiveTo = integrationEvent.ActiveTo;
            DateAssigned = integrationEvent.DateAssigned;
            RoleGiverEmail = integrationEvent.RoleGiverEmail;
            RoleGiverUsername = integrationEvent.RoleGiverUsername;
            RoleGiverId = integrationEvent.RoleGiverId;
        }

        [JsonConstructor]
        public RoleAssignedToUserNotification(Guid userId, string userName, string userEmail, long roleId,
            string roleName, string roleGiverEmail, string roleGiverUsername, Guid roleGiverId, DateTimeOffset activeTo,
            DateTimeOffset dateAssigned) : base(null)
        {
            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
            RoleName = roleName;
            RoleId = roleId;
            RoleActiveTo = activeTo;
            DateAssigned = dateAssigned;
            RoleGiverEmail = roleGiverEmail;
            RoleGiverUsername = roleGiverUsername;
            RoleGiverId = roleGiverId;
        }
    }
}