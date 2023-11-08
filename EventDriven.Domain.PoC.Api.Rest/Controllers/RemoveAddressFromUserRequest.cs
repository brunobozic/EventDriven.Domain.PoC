using System;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    /// <summary>
    /// </summary>
    public class RemoveAddressFromUserRequest
    {
        /// <summary>
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// </summary>
        public string AddressName { get; set; }

        /// <summary>
        /// </summary>
        public Guid RoleId { get; set; }
    }
}