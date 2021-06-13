using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification
{
    public class EmailVerifiedDomainEvent : DomainEventBase
    {
        public EmailVerifiedDomainEvent(string email, long id)
        {
            Email = email;
            Id = id;
        }

        public EmailVerifiedDomainEvent(string email, string userName, long id)
        {
            Email = email;
            UserName = userName;
            Id = id;
        }

        public string Email { get; }
        public long Id { get; }
        public string UserName { get; }
    }
}