using System;
using Microsoft.Extensions.Primitives;
using SharedKernel.DomainContracts;

namespace IdentityService.Application.CommandsAndHandlers.ForgotPassword.PhaseTwo;

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