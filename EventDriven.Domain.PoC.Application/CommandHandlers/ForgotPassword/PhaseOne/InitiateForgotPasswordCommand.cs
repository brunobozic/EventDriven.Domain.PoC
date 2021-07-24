using System;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.ForgotPassword.PhaseOne
{
    public class InitiateForgotPasswordCommand : ICommand<bool>
    {
        internal string Email;
        public string Origin;

        private Guid UserId;
        internal string UserName;

        public InitiateForgotPasswordCommand(Guid userId, string email, string userName)
        {
            Email = email;
            UserId = userId;
            UserName = userName;
        }

        public Guid Id => throw new NotImplementedException();
    }
}