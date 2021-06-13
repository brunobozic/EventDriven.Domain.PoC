namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response
{
    public class CreateApplicationUserResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserViewModel ViewModel { get; set; }
        public string InnerMessage { get; internal set; }
        public string UserFriendlyMessage { get; internal set; }
    }
}