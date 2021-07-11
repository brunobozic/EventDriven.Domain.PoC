using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.VerificationEmail.Notifications
{
    public class EmailVerifiedNotification : IntegrationEventBase<EmailVerifiedDomainEvent>
    {
        public string Email;

        public EmailVerifiedNotification(EmailVerifiedDomainEvent integrationEvent) : base(integrationEvent)
        {
            Email = integrationEvent.Email;
        }

        [JsonConstructor]
        public EmailVerifiedNotification(
            string email
        ) : base(null)
        {
            Email = email;
        }
    }
}