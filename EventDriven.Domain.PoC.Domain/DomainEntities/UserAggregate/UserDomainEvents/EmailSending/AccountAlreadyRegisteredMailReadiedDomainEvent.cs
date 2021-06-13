using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.EmailSending
{
    public class AccountAlreadyRegisteredMailReadiedDomainEvent : DomainEventBase
    {
        public AccountAlreadyRegisteredMailReadiedDomainEvent(
            string email
            , string userName
            , long userId
            , string emailSubject
            , string emailBody
            , string emailFrom
        )
        {
            Email = email;
            UserName = userName;
            UserId = userId;
            EmailFrom = emailFrom;
            EmailSubject = emailSubject;
            EmailBody = emailBody;
        }

        public string EmailFrom { get; set; }

        public string EmailBody { get; set; }

        public string EmailSubject { get; set; }

        public string Email { get; }
        public string UserName { get; }
        public long UserId { get; set; }
    }
}