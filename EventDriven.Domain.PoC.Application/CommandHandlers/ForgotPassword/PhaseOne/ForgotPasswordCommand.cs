using System;
using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.ForgotPassword.PhaseOne
{
    public class ForgotPasswordCommand : ICommand<bool>
    {
 
        private Guid UserId;

        public ForgotPasswordCommand(Guid userId, string email, string userName)
        {
            Email = email;
            UserId = userId;
            UserName = userName;
        }

        internal string Email;
        internal string UserName;
        public string Origin;

        public Guid Id => throw new NotImplementedException();
    }
}