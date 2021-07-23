using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.VerificationEmail.Notifications
{
    public class EmailNotVerifiedNotification : IntegrationEventBase<EmailNotVerifiedDomainEvent>
    {
        public string Email;

        public EmailNotVerifiedNotification(EmailNotVerifiedDomainEvent integrationEvent) : base(integrationEvent)
        {
            Email = integrationEvent.Email;
        }

        [JsonConstructor]
        public EmailNotVerifiedNotification(
            string email
        ) : base(null)
        {
            Email = email;
        }
    }
}