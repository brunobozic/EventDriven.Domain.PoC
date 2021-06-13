using System;
using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.Email
{
    public class SendAccountAlreadyRegisteredMailReadiedCommand : ICommand<bool>
    {
        private Guid guid;
        private UserId userId;

        public SendAccountAlreadyRegisteredMailReadiedCommand(Guid guid, UserId userId)
        {
            this.guid = guid;
            this.userId = userId;
        }

        public SendAccountAlreadyRegisteredMailReadiedCommand(Guid guid, long userId)
        {
            this.guid = guid;
            UserId = userId;
        }

        public long UserId { get; }
        public Guid Id { get; }
    }
}