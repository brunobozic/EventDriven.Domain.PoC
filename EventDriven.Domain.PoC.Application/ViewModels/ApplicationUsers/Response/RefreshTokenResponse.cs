namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response
{
    public class RefreshTokenResponse
    {
        public bool Success { get; set; }
        public object Message { get; set; }
        public string InnerMessage { get; set; }
        public string UserFriendlyMessage { get; set; }
        public RefreshTokenViewModel ViewModel { get; set; }
    }
}