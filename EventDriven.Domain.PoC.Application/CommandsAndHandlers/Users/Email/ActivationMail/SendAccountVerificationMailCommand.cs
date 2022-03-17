using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email.ActivationMail
{
    public class SendAccountVerificationMailCommand : CommandBase<AccountVerificationMailSentDto>
    {
        public string ActivationLink;
        public DateTimeOffset? ActivationLinkGenerated;
        public string Email;
        public string FirstName;
        public string LastName;

        public string Origin;
        public Guid UserId;
        public string UserName;

        public SendAccountVerificationMailCommand(
            string activationLink,
            DateTimeOffset? activationLinkGenerated,
            Guid userId,
            string email,
            string userName,
            string firstName,
            string lastName,
            string origin
        )
        {
            ActivationLink = activationLink;
            ActivationLinkGenerated = activationLinkGenerated;
            UserId = userId;
            Email = email;
            FirstName = firstName;
            UserName = userName;
            LastName = lastName;
            Origin = origin;
        }
    }
}