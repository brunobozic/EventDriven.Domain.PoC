using System;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    public class AssignRoleToUserRequest
    {
        public Guid UserIdToAssignTo { get; set; }

        public string RoleName { get; set; }
    }
}