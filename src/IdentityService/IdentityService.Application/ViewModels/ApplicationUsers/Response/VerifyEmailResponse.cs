namespace IdentityService.Application.ViewModels.ApplicationUsers.Response;

public class VerifyEmailResponse
{
    public bool Success { get; internal set; } = false;
    public string Message { get; internal set; }
    public string InnerMessage { get; internal set; }
    public string UserFriendlyMessage { get; internal set; }
}