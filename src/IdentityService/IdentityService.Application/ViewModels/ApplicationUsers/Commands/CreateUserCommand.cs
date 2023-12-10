using System;
using System.ComponentModel.DataAnnotations;
using IdentityService.Domain.DomainEntities.UserAggregate;
using SharedKernel.DomainImplementations.BaseClasses;
using SharedKernel.Helpers;

namespace IdentityService.Application.ViewModels.ApplicationUsers.Commands;

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
        , User creatorUser
        , string origin
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
        if (creatorUser != null)
            Creator = creatorUser;
    }

    public DateTimeOffset ActiveFrom { get; set; }
    public DateTimeOffset ActiveTo { get; set; }
    [Required] [Compare("Password")] public string ConfirmPassword { get; set; }
    public User Creator { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    [EmailAddress] public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Oib { get; set; }
    public string Origin { get; set; }
    public string Password { get; set; }
    [EnumDataType(typeof(RoleEnum))] public string Role { get; set; }
    public string Title { get; set; }

    public string UserName { get; set; }
    // helpers

    private string replaceEmptyWithNull(string value)
    {
        // replace empty string with null to make field optional
        return string.IsNullOrEmpty(value) ? null : value;
    }
}