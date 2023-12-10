namespace IdentityService.Application.ViewModels.ApplicationUsers.Request;

public class AuthenticateRequest
{
    public string Email { get; set; }

    public string Password { get; set; }
}