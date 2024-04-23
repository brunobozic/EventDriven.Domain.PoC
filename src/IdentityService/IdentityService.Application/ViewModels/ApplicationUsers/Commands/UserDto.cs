using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using System;
using System.Collections.Generic;

namespace IdentityService.Application.ViewModels.ApplicationUsers.Commands;

public sealed record UserDto(
    DateTimeOffset? ActiveTo,
    string Email,
    DateTime HasBeenVerified,
    Guid? Id,
    string Status,
    string UserName,
    List<Role> UserRoles);