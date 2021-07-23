using System;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.SharedKernel.Helpers;

namespace EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request
{
    public class CreateApplicationUserRequest
    {
        public string Title { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EnumDataType(typeof(RoleEnum))] public string Role { get; set; }

        [EmailAddress] public string Email { get; set; }

        public string Password { get; set; }

        [Required] [Compare("Password")] public string ConfirmPassword { get; set; }

        public DateTimeOffset? DateOfBirth { get; set; }
        public string Origin { get; internal set; }
        public Guid Id { get; internal set; }

        // helpers

        private string replaceEmptyWithNull(string value)
        {
            // replace empty string with null to make field optional
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}