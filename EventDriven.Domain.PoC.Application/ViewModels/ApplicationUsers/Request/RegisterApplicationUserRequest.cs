using System;
using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request
{
    public class RegisterApplicationUserRequest
    {
        public string UserName { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [Compare("Password")] public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true")] public bool AcceptTerms { get; set; }

        public string Oib { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public object Role { get; set; }
    }
}