using System;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    /// <summary>
    /// </summary>
    public class RemoveRoleFromUserRequest
    {
        /// <summary>
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// </summary>
        public Guid UserIdToRemoveFrom { get; set; }
    }
}