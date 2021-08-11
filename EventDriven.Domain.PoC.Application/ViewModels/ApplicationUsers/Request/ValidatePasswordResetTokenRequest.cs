using System;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request
{
    public class ValidatePasswordResetTokenRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}