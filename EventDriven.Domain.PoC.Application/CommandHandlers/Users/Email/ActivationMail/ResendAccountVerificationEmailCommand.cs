using System;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.Email.ActivationMail
{
    public class ResendAccountVerificationEmailCommand : ICommand<bool>
    {
        public string Email;
        public Guid UserId;
        public string UserName;

        public ResendAccountVerificationEmailCommand(string userEmailAddress)
        {
            Email = userEmailAddress;
        }

        public string Origin { get; set; }

        public Guid Id => throw new NotImplementedException();
    }
}