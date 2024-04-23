using System;

namespace IdentityService.Application.ViewModels.ApplicationUsers.Request;

public sealed record ValidatePasswordResetTokenRequest
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
}