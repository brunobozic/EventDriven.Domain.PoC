using System;
using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request
{
    public class RegisterUserRequest
    {
        public RegisterUserRequest(
            string email
            , string confirmPassword
            , DateTimeOffset? dateOfBirth
            , string firstName
            , string lastName
            , string password
            , string userName
            , string oib
        )
        {
            Email = email;
            ConfirmPassword = confirmPassword;
            DateOfBirth = dateOfBirth;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            UserName = userName;
            Oib = oib;
        }

        public DateTimeOffset? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Oib { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        [Compare("Password")] public string ConfirmPassword { get; set; }

        [Range(typeof(bool), "true", "true")] public bool AcceptTerms { get; set; }
    }
}