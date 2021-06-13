using System;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.CUD
{
    public class RegisterUserCommand : CommandBase<UserDto>
    {
        public string ConfirmPassword;
        public DateTimeOffset? DateOfBirth;
        public string Email;
        public string FirstName;
        public string LastName;
        public string Oib;
        public string Password;
        public string UserName;

        public RegisterUserCommand(
            string email
            , string confirmPassword
            , DateTimeOffset? dateOfBirth
            , string firstName
            , string lastName
            , string password
            , string userName
            , string oib
            , long? userId
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
            CreatorId = userId;
        }

        public long? CreatorId { get; set; }
    }
}