using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email.ActivationMail
{
    public class SendAccountVerificationMailCommand : CommandBase<AccountVerificationMailSentDto>
    {
        public string ActivationLink;
        public DateTimeOffset? ActivationLinkGenerated;
        public string Email;
        public string FirstName;
        public string LastName;
        public Guid UserId;

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

        public string Origin;
        public string UserName;
    }
}