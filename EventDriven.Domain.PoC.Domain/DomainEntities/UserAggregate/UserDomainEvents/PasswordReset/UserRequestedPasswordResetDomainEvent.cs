using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.PasswordReset
{
    public class UserRequestedPasswordResetDomainEvent : DomainEventBase
    {
        public UserRequestedPasswordResetDomainEvent(string email, string userName, long id, string randomTokenString)
        {
            Email = email;
            UserName = userName;
            Id = id;
            RandomTokenString = randomTokenString;
        }

        public string Email { get; }
        public string UserName { get; }
        public long Id { get; }
        public string RandomTokenString { get; }
    }
}