using System;
using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.Extensions.Primitives;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.ForgotPassword.PhaseTwo
{
    public class ValidatePasswordResetTokenCommand : ICommand<bool>
    {
        private Guid guid;
        private UserId userId;

        public ValidatePasswordResetTokenCommand(Guid guid, UserId userId)
        {
            this.guid = guid;
            this.userId = userId;
        }

        public StringValues Origin { get; set; }
        public string Token { get; internal set; }

        public Guid Id => throw new NotImplementedException();
    }
}