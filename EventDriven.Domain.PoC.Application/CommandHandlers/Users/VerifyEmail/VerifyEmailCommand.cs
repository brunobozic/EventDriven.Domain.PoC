using System;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.VerifyEmail
{
    public class VerifyEmailCommand : ICommand<VerifyEmailResponse>
    {
        public VerifyEmailCommand(string emailVerificationToken)
        {
            EmailVerificationToken = emailVerificationToken;
        }

        public string EmailVerificationToken { get; set; }
        public string Origin { get; set; } = "";

        public Guid Id { get; set; } = Guid.NewGuid();
    }
}