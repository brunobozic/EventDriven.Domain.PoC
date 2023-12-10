using System;
using System.Collections.Generic;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;

namespace IdentityService.Application.ViewModels.ApplicationUsers.Commands;

public record UserDto(
    DateTimeOffset? ActiveTo,
    string Email,
    DateTime HasBeenVerified,
    Guid? Id,
    string Status,
    string UserName,
    List<Role> UserRoles);