using System;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application;

public class RegisterUserCommand : CommandBase<UserDto>
{
    public RegisterUserCommand(
        Guid userId
        , string email
        , string confirmPassword
        , DateTimeOffset? dateOfBirth
        , string firstName
        , string lastName
        , string password
        , string userName
        , string oib
    )
    {
        UserId = UserId;
        Email = email;
        ConfirmPassword = confirmPassword;
        DateOfBirth = dateOfBirth;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        UserName = userName;
        Oib = oib;
    }

    public string ConfirmPassword { get; set; }
    public Guid? CreatorId { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Oib { get; set; }
    public string Origin { get; set; }
    public string Password { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
}