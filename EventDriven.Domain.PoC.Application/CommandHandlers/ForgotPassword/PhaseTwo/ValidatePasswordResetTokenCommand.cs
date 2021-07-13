using System;
using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.Extensions.Primitives;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.ForgotPassword.PhaseTwo
{
    public class ValidatePasswordResetTokenCommand : ICommand<bool>
    {

        private Guid UserId;

        public ValidatePasswordResetTokenCommand(Guid userId, string token)
        {
            this.Token = token;
            this.UserId = userId;
        }

        public StringValues Origin { get; set; }
        public string Token { get; set; }

        public Guid Id => throw new NotImplementedException();
    }
}