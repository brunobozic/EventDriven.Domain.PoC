using System;

namespace IdentityService.Api.Controllers;

public class AssignRoleToUserRequest
{
    public Guid UserIdToAssignTo { get; set; }

    public string RoleName { get; set; }
}