using System;

namespace IdentityService.Application.ViewModels;

public sealed record RemoveAddressFromUserRequest
{
    public Guid UserId { get; set; }

    public string AddressName { get; set; }

    public Guid RoleId { get; set; }
}