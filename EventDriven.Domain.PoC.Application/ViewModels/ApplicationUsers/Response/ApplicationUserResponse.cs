using System.Collections.Generic;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response
{
    public class ApplicationUserResponse
    {
        public bool Success { get; set; }
        public object Message { get; set; }
        public string InnerMessage { get; set; }
        public string UserFriendlyMessage { get; set; }
        public UserViewModel ViewModel { get; set; }
        public List<UserViewModel> ViewModels { get; set; }
    }
}