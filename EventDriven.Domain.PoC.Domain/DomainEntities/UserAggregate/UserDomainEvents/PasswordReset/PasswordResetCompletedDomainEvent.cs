using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.PasswordReset
{
    public class PasswordResetCompletedDomainEvent : DomainEventBase
    {
        public PasswordResetCompletedDomainEvent(string email, string userName, Guid id)
        {
            Email = email;
            UserName = userName;
            Id = id;
        }

        public string Email { get; }
        public string UserName { get; }
        public Guid Id { get; }
    }
}