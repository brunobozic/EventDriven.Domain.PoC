using System;

namespace IdentityService.Application.ViewModels;

public sealed record AssignRoleToUserRequest
{
    public Guid UserIdToAssignTo { get; set; }

    public string RoleName { get; set; }
}