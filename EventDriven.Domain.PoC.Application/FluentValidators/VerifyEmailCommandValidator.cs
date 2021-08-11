using EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.VerifyEmail;
using FluentValidation;

namespace EventDriven.Domain.PoC.Application.FluentValidators
{
    public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(ou => ou.EmailVerificationToken).NotEmpty()
                .WithMessage("Email verification token cannot be empty.");
        }
    }
}