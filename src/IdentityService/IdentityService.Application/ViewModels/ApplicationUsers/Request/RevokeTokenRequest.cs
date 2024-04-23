using System;

namespace IdentityService.Application.ViewModels.ApplicationUsers.Request;

public sealed record RevokeTokenRequest
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
}