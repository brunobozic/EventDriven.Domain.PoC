using System;
using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.ForgotPassword.PhaseOne
{
    public class ForgotPasswordCommand : ICommand<bool>
    {
        private Guid guid;
        private UserId userId;

        public ForgotPasswordCommand(Guid guid, UserId userId)
        {
            this.guid = guid;
            this.userId = userId;
        }

        public string Email { get; set; }
        public string UserName { get; set; }
        public string Origin { get; set; }

        public Guid Id => throw new NotImplementedException();
    }
}