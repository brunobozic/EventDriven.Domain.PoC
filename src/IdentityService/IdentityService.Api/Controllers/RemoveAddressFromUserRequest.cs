using System;

namespace IdentityService.Api.Controllers;

public class RemoveAddressFromUserRequest
{
    public Guid UserId { get; set; }

    public string AddressName { get; set; }

    public Guid RoleId { get; set; }
}