using EventDriven.Domain.PoC.SharedKernel.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request
{
    public class UpdateApplicationUserRequest
    {
        private string _confirmPassword;
        private string _email;
        private string _password;
        private string _role;
        private string firstName;
        private string lastName;
        private string oib;
        private string title;
        private string userName;

        public DateTimeOffset? DateOfBirth { get; set; }

        public string UserName
        {
            get => userName;
            set => userName = replaceEmptyWithNull(value);
        }

        public string Oib
        {
            get => oib;
            set => oib = replaceEmptyWithNull(value);
        }

        public string Title
        {
            get => title;
            set => title = replaceEmptyWithNull(value);
        }

        public string FirstName
        {
            get => firstName;
            set => firstName = replaceEmptyWithNull(value);
        }

        public string LastName
        {
            get => lastName;
            set => lastName = replaceEmptyWithNull(value);
        }

        [EnumDataType(typeof(RoleEnum))]
        public string Role
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

        // helpers

        private string replaceEmptyWithNull(string value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}