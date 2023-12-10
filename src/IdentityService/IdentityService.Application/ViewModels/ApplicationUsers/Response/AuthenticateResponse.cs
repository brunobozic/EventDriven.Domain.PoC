using System;
using System.Text.Json.Serialization;

namespace IdentityService.Application.ViewModels.ApplicationUsers.Response;

public class AuthenticateResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public bool IsVerified { get; set; } = false;
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }

    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; } = false;
    public string InnerMessage { get; set; } = string.Empty;
    public string UserFriendlyMessage { get; set; } = string.Empty;
}