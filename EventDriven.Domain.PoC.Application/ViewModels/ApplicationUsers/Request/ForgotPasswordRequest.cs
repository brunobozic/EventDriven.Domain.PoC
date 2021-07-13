using System;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}