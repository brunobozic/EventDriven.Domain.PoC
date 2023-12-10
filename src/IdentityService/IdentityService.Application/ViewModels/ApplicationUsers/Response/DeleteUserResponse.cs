namespace IdentityService.Application.ViewModels.ApplicationUsers.Response;

public class DeleteUserResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string InnerMessage { get; set; }
    public UserViewModel ViewModel { get; set; }
}