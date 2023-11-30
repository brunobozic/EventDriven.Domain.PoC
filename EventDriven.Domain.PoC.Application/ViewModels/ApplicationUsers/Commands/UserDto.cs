using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using System;
using System.Collections.Generic;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands
{
    public record UserDto(
        DateTimeOffset? ActiveTo,
        string Email,
        DateTime HasBeenVerified,
        Guid? Id,
        string Status,
        string UserName,
        List<Role> UserRoles);
}