using System;
using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.PasswordReset
{
    public class InitiatePasswordResetCommand : ICommand<object>
    {
        private Guid guid;
        private Guid userId;

        public InitiatePasswordResetCommand(Guid guid, Guid userId)
        {
            this.guid = guid;
            this.userId = userId;
        }

        public Guid Id => throw new NotImplementedException();
    }
}