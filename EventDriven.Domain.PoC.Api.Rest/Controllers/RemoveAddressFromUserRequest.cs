using System;

namespace EventDriven.Domain.PoC.Api.Rest.Controllers
{
    public class RemoveAddressFromUserRequest
    {
        public Guid UserId { get; set; }

        public string AddressName { get; set; }

        public Guid RoleId { get; set; }
    }
}