using System;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.VerifyEmail
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

        public string UserName { get; set; }
        public string EmailVerificationToken { get; set; }
        public string Origin { get; set; } = "";
        public Guid UserId { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get;  set; }
    }
}