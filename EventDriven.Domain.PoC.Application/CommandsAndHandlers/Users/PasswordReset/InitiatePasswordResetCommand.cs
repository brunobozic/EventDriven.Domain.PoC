using System;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.PasswordReset
{
    public class InitiatePasswordResetCommand : ICommand<object>, ICommand<bool>
    {
        private Guid guid;
        private Guid userId;

        public InitiatePasswordResetCommand(Guid guid, Guid userId)
        {
            this.guid = guid;
            this.userId = userId;
        }

        public Guid Id => Guid.NewGuid();
    }
}