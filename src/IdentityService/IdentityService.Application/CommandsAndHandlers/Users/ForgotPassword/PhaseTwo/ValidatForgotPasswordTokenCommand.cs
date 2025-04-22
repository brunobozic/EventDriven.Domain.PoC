using Microsoft.Extensions.Primitives;
using SharedKernel.DomainContracts;
using System;

namespace IdentityService.Application.CommandsAndHandlers.Users.ForgotPassword.PhaseTwo;

public class ValidatForgotPasswordTokenCommand : ICommand<bool>
{
    private Guid UserId;

    public ValidatForgotPasswordTokenCommand(Guid userId, string token)
    {
        Token = token;
        UserId = userId;
    }

    public StringValues Origin { get; set; }
    public string Token { get; set; }

    public Guid Id => throw new NotImplementedException();
}