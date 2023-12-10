using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;

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