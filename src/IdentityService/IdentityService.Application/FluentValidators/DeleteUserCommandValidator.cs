using FluentValidation;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;
using System;
using System.Text.RegularExpressions;

namespace IdentityService.Application.FluentValidators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    private static Regex ValidEmailRegex = CreateValidEmailRegex();

    private static Regex CreateValidEmailRegex()
    {
        throw new NotImplementedException();
    }
}