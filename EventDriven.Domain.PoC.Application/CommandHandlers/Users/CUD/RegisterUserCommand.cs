using System;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.CUD
{
    public class RegisterUserCommand : CommandBase<UserDto>
    {
        public RegisterUserCommand(
            string email
            , string confirmPassword
            , DateTimeOffset? dateOfBirth
            , string firstName
            , string lastName
            , string password
            , string userName
            , string oib
            , Guid userId
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

        public string ConfirmPassword { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Oib { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public Guid CreatorId { get; set; }
        public string Origin { get; set; }
    }
}