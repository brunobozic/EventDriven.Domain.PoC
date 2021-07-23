using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Request;
using FluentValidation;

namespace EventDriven.Domain.PoC.Application.FluentValidators
{
    public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
    {
        public VerifyEmailRequestValidator()
        {
            RuleFor(ou => ou.EmailVerificationToken).NotEmpty()
                .WithMessage("Email verification token cannot be empty.");
        }
    }
}