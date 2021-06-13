using System;
using System.Text.RegularExpressions;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands;
using FluentValidation;

namespace EventDriven.Domain.PoC.Application.FluentValidators
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        private static Regex ValidEmailRegex = CreateValidEmailRegex();

        private static Regex CreateValidEmailRegex()
        {
            throw new NotImplementedException();
        }
    }
}