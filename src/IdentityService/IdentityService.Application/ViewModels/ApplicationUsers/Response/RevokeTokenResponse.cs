namespace IdentityService.Application.ViewModels.ApplicationUsers.Response;

public sealed record RevokeTokenResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string InnerMessage { get; set; }
    public string UserFriendlyMessage { get; set; }
}