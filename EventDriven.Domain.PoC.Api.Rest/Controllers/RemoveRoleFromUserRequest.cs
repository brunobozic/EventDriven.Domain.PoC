using System;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    public class RemoveRoleFromUserRequest
    {
        public string RoleName { get; set; }

        public Guid UserIdToRemoveFrom { get; set; }
    }
}