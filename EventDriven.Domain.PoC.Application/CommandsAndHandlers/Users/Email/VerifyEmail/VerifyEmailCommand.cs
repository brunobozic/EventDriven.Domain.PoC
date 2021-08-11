using System;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.VerifyEmail
{
    public class VerifyEmailCommand : ICommand<VerifyEmailResponse>
    {
        public VerifyEmailCommand(string emailVerificationToken, Guid userId, string userEmail, string userName)
        {
            EmailVerificationToken = emailVerificationToken;
            Email = userEmail;
            UserId = userId;
            UserName = userName;
        }

        public string UserName;
        public string EmailVerificationToken;
        public string Origin { get; set; } = "";
        public Guid UserId;
        public string Email;
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}