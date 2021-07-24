using System;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.Extensions.Primitives;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.ForgotPassword.PhaseTwo
{
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
}