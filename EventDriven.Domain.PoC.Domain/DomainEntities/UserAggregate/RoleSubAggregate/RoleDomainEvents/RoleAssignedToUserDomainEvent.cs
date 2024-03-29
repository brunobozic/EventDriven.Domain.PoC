﻿using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents
{
    public class RoleAssignedToUserDomainEvent : DomainEventBase
    {
        public DateTimeOffset? ActiveTo;
        public DateTimeOffset DateAssigned;
        public string Email;
        public string Name;
        public string RoleGiverEmail;
        public long RoleGiverId;
        public string RoleGiverUsername;
        public long RoleId;
        public long UserId;
        public string UserName;
        public DateTimeOffset UtcNow;

        public RoleAssignedToUserDomainEvent(
            long userId
            , string userName
            , string email
            , long roleId
            , string name
            , DateTimeOffset? activeTo
            , long roleGiverId
            , string roleGiverEmail
            , string roleGiverUsername
            , DateTimeOffset utcNow
        )
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            RoleId = roleId;
            Name = name;
            ActiveTo = activeTo;
            RoleGiverId = roleGiverId;
            RoleGiverEmail = roleGiverEmail;
            RoleGiverUsername = roleGiverUsername;
            UtcNow = utcNow;
        }
    }
}