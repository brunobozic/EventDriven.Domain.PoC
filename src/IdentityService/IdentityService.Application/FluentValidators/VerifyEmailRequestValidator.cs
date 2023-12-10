using FluentValidation;
using IdentityService.Application.ViewModels.ApplicationUsers.Request;

namespace IdentityService.Application.FluentValidators;

public class VerifyEmailRequestValidator : AbstractValidator<VerifyEmailRequest>
{
    public VerifyEmailRequestValidator()
    {
        RuleFor(ou => ou.EmailVerificationToken).NotEmpty()
            .WithMessage("Email verification token cannot be empty.");
    }
}