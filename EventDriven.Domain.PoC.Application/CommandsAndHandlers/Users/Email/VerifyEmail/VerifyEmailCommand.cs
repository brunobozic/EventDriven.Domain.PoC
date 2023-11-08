using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using System;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.VerifyEmail
{
    public class VerifyEmailCommand : ICommand<VerifyEmailResponse>
    {
        public string Email;
        public string EmailVerificationToken;
        public Guid UserId;

        public string UserName;

        public VerifyEmailCommand(string emailVerificationToken, Guid userId, string userEmail, string userName)
        {
            EmailVerificationToken = emailVerificationToken;
            Email = userEmail;
            UserId = userId;
            UserName = userName;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Origin { get; set; } = "";
    }
}