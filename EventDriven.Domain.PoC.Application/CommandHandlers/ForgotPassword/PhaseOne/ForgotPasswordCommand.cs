using System;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.ForgotPassword.PhaseOne
{
    public class ForgotPasswordCommand : ICommand<bool>
    {
        internal string Email;
        public string Origin;

        private Guid UserId;
        internal string UserName;

        public ForgotPasswordCommand(Guid userId, string email, string userName)
        {
            Email = email;
            UserId = userId;
            UserName = userName;
        }

        public Guid Id => throw new NotImplementedException();
    }
}