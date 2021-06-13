using System;
using System.Text.RegularExpressions;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request;
using FluentValidation;

namespace EventDriven.Domain.PoC.Application.FluentValidators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateApplicationUserRequest>
    {
        private static readonly Regex ValidEmailRegex = CreateValidEmailRegex();

        public CreateUserRequestValidator()
        {
            RuleFor(ou => ou.FirstName).NotEmpty().WithMessage("FirstName cannot be empty");
            RuleFor(ou => ou.LastName).NotEmpty().WithMessage("LastName cannot be empty");
            // RuleFor(ou => ou.Role).NotEmpty().WithMessage("Role cannot be empty");
            RuleFor(ou => ou.Email).NotEmpty().WithMessage("Email cannot be empty");

            //RuleFor(ou => ou.ActiveFrom).NotNull().ActiveFrom("ActiveFrom cannot be null");

            //RuleFor(ou => ou.ActiveFrom).InclusiveBetween(DateTimeOffset.UtcNow.AddMinutes(-60), DateTimeOffset.UtcNow.AddMinutes(5))
            //        .WithMessage("ActiveFrom value must be a valid date between now minus 60 minutes and 5 minutes in the future.");

            //RuleFor(ou => ou.ActiveTo).NotNull().ActiveFrom("ActiveTo cannot be null");

            //RuleFor(ou => ou.ActiveTo).InclusiveBetween(DateTimeOffset.UtcNow.AddMinutes(-60), DateTimeOffset.UtcNow.AddMinutes(5))
            //        .WithMessage("ActiveTo value must be a valid date between now minus 60 minutes and 5 minutes in the future.");

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
        //    var result = _c.Users.Where(x => x.UserName.ToUpper() == userName.ToLower()).FirstOrDefault();

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
}