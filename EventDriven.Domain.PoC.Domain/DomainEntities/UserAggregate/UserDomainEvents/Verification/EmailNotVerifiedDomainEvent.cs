using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification
{
    public class EmailNotVerifiedDomainEvent : DomainEventBase
    {
        public EmailNotVerifiedDomainEvent(string email, Guid id, string verificationFailureLatestMessage)
        {
            Email = email;
            Id = id;
        }

        public EmailNotVerifiedDomainEvent(string email, string userName, Guid id,
            string verificationFailureLatestMessage)
        {
            Email = email;
            UserName = userName;
            Id = id;
            VerificationFailureLatestMessage = verificationFailureLatestMessage;
        }

        public string Email { get; }
        public Guid Id { get; }
        public string VerificationFailureLatestMessage { get; }
        public string UserName { get; }
    }
}