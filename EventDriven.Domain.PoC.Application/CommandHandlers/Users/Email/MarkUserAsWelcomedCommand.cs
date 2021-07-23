using System;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.Email
{
    public class MarkUserAsWelcomedCommand : ICommand<object>
    {
        private Guid guid;
        private Guid userId;

        public MarkUserAsWelcomedCommand(Guid guid, Guid userId)
        {
            this.guid = guid;
            this.userId = userId;
        }

        public Guid Id => throw new NotImplementedException();
    }
}