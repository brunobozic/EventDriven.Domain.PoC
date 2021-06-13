using System;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using EventDriven.Domain.PoC.SharedKernel.Helpers;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands
{
    public class UpdateUserCommand : CommandBase<UserDto>
    {
        private string _confirmPassword;
        private string _email;
        private string _password;
        private string _role;

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EnumDataType(typeof(RoleEnum))]
        public string RoleEnum
        {
            get => _role;
            set => _role = replaceEmptyWithNull(value);
        }

        [EmailAddress]
        public string Email
        {
            get => _email;
            set => _email = replaceEmptyWithNull(value);
        }

        public string Password
        {
            get => _password;
            set => _password = replaceEmptyWithNull(value);
        }

        [Compare("Password")]
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => _confirmPassword = replaceEmptyWithNull(value);
        }

        public DateTimeOffset? DateOfBirth { get; set; }
        public string UserName { get; set; }
        public object Oib { get; internal set; }

        // helpers

        private string replaceEmptyWithNull(string value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}