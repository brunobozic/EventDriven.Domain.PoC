using System;

namespace IdentityService.Api.Controllers;

public class RemoveRoleFromUserRequest
{
    public string RoleName { get; set; }

    public Guid UserIdToRemoveFrom { get; set; }
}