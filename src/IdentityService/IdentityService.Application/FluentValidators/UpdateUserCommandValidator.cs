using System;
using System.Text.RegularExpressions;
using FluentValidation;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;

namespace IdentityService.Application.FluentValidators;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    private static readonly Regex ValidEmailRegex = CreateValidEmailRegex();

    public UpdateUserCommandValidator()
    {
        RuleFor(ou => ou.FirstName).NotEmpty().WithMessage("FirstName cannot be empty");
        RuleFor(ou => ou.LastName).NotEmpty().WithMessage("LastName cannot be empty");
        RuleFor(ou => ou.Email).NotEmpty().WithMessage("Email cannot be empty");
        RuleFor(ou => ou.Oib).NotEmpty().WithMessage("Oib cannot be empty");

        RuleFor(ou => ou.DateOfBirth)
            .Must(IsNotTooOld)
            .WithMessage("DateOfBirth value must be a valid date and not too old when .")
            ;

        RuleFor(ou => ou.Password).NotEmpty().WithMessage("Password cannot be empty");
        RuleFor(ou => ou.Password).MinimumLength(9).WithMessage("Password cannot be less than 9 chars long");
        RuleFor(ou => ou.Password).MaximumLength(14).WithMessage("Password cannot be more than 14 chars long");

        RuleFor(ou => ou.ConfirmPassword).NotEmpty().WithMessage("ConfirmPassword cannot be empty");
        RuleFor(ou => ou.ConfirmPassword).MinimumLength(9)
            .WithMessage("ConfirmPassword cannot be less than 9 chars long");
        RuleFor(ou => ou.ConfirmPassword).MaximumLength(14)
            .WithMessage("ConfirmPassword cannot be more than 14 chars long");

        RuleFor(ou => ou.DateOfBirth)
            .Must(IsNotTooOld)
            .WithMessage("DateOfBirth value must be a valid date and not too old when .")
            // .When(ou => ou.Akcija.ToDescriptionString() == "")
            ;

        RuleFor(ou => ou.UserName)
            // .Must(BeUnique)
            .Must(IsValidUsername)
            .WithMessage("UserName value must be unique.");

        RuleFor(ou => ou.UserName)
            .Must(IsValidUsername)
            .WithMessage("UserName value must be valid (no special characters allowed");

        RuleFor(ou => ou.Email)
            .Must(IsValidEmailAddress)
            .WithMessage("Email must be valid.");

        // RuleFor(ou => ou.Id).NotNull().GreaterThan(0).WithMessage("Id cannot be empty or 0");
    }

    private bool IsNotTooOld(DateTimeOffset? date)
    {
        return DateTime.Now.AddMinutes(-18) <= date;
    }

    //private bool BeUnique(string userName)
    //{
    //    var result = _c.Users
    //                        .Where(x => x.UserName.ToUpper() == userName.ToLower())
    //                        .FirstOrDefault();

    //    if (result == null)
    //        return true;

    //    return false;
    //}

    private bool IsValidUsername(string username)
    {
        var specialCharacterPattern = new Regex("^[a-zA-Z0-9 ]*$");

        if (specialCharacterPattern.IsMatch(username))
            return true;

        return false;
    }

    private static Regex CreateValidEmailRegex()
    {
        var validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
    }

    private bool IsValidEmailAddress(string emailAddress)
    {
        return !string.IsNullOrEmpty(emailAddress) &&
               ValidEmailRegex.IsMatch(emailAddress);
    }

    private bool IsAValidMobilePhoneNumber(string mobilePhoneNumber)
    {
        if (!string.IsNullOrEmpty(mobilePhoneNumber) &&
            mobilePhoneNumber.StartsWith("+385", StringComparison.CurrentCulture) &&
            mobilePhoneNumber.Length == 13)
            return true;

        return false;
    }
}