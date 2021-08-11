using System;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email.ActivationMail
{
    public class ResendAccountVerificationEmailCommand : ICommand<bool>
    {
        public string Email;
        public string FirstName;
        public string LastName;
        public Guid UserId;
        public string Origin;

        public ResendAccountVerificationEmailCommand(string email, string userName)
        {
            Email = email;
            UserName = userName;
        }

        public Guid Id => Guid.NewGuid();
        public string UserName;
    }
}