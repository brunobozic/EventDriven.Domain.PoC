using System;
using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.Extensions.Primitives;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.Email.ActivationMail
{
    public class ResendAccountActivationCommand : ICommand<bool>
    {
        public Guid UserId;
        public string UserName;
        public string Email;
        public ResendAccountActivationCommand(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid Id => throw new NotImplementedException();
        public string Origin { get; set; }
    }
}