using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification
{
    public class EmailVerifiedDomainEvent : DomainEventBase
    {
        public EmailVerifiedDomainEvent(string email, Guid id)
        {
            Email = email;
            Id = id;
        }

        public EmailVerifiedDomainEvent(string email, string userName, Guid id, DateTimeOffset verifiedOffset)
        {
            Email = email;
            UserName = userName;
            Id = id;
            VerifiedDate = verifiedOffset;
        }

        public DateTimeOffset VerifiedDate { get; }
        public string Email { get; }
        public Guid Id { get; }
        public string UserName { get; }
    }
}