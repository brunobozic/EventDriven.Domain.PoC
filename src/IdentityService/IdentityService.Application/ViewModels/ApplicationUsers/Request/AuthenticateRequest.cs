namespace IdentityService.Application.ViewModels.ApplicationUsers.Request;

public sealed record AuthenticateRequest
{
    public string Email { get; set; }

    public string Password { get; set; }
}