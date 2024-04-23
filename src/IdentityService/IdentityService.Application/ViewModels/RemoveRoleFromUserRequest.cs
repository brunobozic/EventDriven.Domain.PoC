using System;

namespace IdentityService.Application.ViewModels;

public sealed record RemoveRoleFromUserRequest
{
    public string RoleName { get; set; }

    public Guid UserIdToRemoveFrom { get; set; }
}