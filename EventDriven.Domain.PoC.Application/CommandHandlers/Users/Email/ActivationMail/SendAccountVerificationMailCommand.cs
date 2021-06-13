using System;
using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.Email.ActivationMail
{
    public class SendAccountVerificationMailCommand : CommandBase<AccountVerificationMailSentDto>
    {
        public string ActivationLink;
        public DateTimeOffset? ActivationLinkGenerated;
        public string Email;
        public string FirstName;
        public Guid Id;
        public string LastName;
        public UserId UserId;

        public SendAccountVerificationMailCommand(
            Guid id,
            string activationLink,
            DateTimeOffset? activationLinkGenerated,
            UserId userId,
            string email,
            string firstName,
            string lastName
        )
        {
            Id = id;
            ActivationLink = activationLink;
            ActivationLinkGenerated = activationLinkGenerated;
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}