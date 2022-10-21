﻿using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Roles
{
    public class RemoveRoleFromUserCommand : CommandBase<ApplicationRoleAssignmentDto>
    {
        public RemoveRoleFromUserCommand(
            Guid userId
            , string roleName
        )
        {
            UserId = userId;
            RoleName = roleName;
        }

        public string RoleName { get; set; }

        public Guid UserId { get; set; }
        public User RemoverUser { get; set; }
        public string Origin { get; set; }
    }
}