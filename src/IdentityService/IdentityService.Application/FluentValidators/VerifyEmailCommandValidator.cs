using FluentValidation;
using IdentityService.Application.CommandsAndHandlers.Users.Email.VerifyEmail;

namespace IdentityService.Application.FluentValidators;

public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(ou => ou.EmailVerificationToken).NotEmpty()
            .WithMessage("Email verification token cannot be empty.");
    }
}