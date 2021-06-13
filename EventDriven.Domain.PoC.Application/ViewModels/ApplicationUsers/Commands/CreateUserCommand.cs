using System;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using EventDriven.Domain.PoC.SharedKernel.Helpers;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands
{
    public class CreateUserCommand : CommandBase<UserDto>
    {
        public CreateUserCommand(
            string email
            , string confirmPassword
            , DateTimeOffset? dateOfBirth
            , string firstName
            , string lastName
            , string password
            , string role
            , string title
            , string userName
            , User user
        )
        {
            Email = email;
            ConfirmPassword = confirmPassword;
            DateOfBirth = dateOfBirth;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Role = role;
            Title = title;
            UserName = userName;
            if (user != null)
                Creator = user;
        }

        public string Title { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EnumDataType(typeof(RoleEnum))] public string Role { get; set; }

        [EmailAddress] public string Email { get; set; }

        public string Password { get; set; }

        [Required] [Compare("Password")] public string ConfirmPassword { get; set; }

        public DateTimeOffset? DateOfBirth { get; set; }
        public string Oib { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset ActiveTo { get; set; }
        public User Creator { get; set; }


        // helpers

        private string replaceEmptyWithNull(string value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}