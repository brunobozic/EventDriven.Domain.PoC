﻿using System;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Roles.Notifications
{
    public class RoleAssignedToUserNotification : IntegrationEventBase<RoleAssignedToUserDomainEvent>
    {
        public DateTimeOffset DateAssigned;
        public DateTimeOffset? RoleActiveTo;
        public string RoleGiverEmail;
        public long RoleGiverId;
        public string RoleGiverUsername;
        public long RoleId;
        public string RoleName;
        public string UserEmail;
        public long UserId;
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
        public RoleAssignedToUserNotification(long userId, string userName, string userEmail, long roleId,
            string roleName, string roleGiverEmail, string roleGiverUsername, long roleGiverId, DateTimeOffset activeTo,
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