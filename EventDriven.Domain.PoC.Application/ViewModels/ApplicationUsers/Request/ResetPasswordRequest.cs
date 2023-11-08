using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; }

        public string Password { get; set; }

        [Required][Compare("Password")] public string ConfirmPassword { get; set; }
    }
}