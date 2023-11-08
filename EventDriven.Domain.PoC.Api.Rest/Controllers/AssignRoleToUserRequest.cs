using System;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    /// <summary>
    /// </summary>
    public class AssignRoleToUserRequest
    {
        /// <summary>
        /// </summary>
        public Guid UserIdToAssignTo { get; set; }

        /// <summary>
        /// </summary>
        public string RoleName { get; set; }
    }
}