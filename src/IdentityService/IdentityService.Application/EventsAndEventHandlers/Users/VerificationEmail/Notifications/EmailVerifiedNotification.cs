using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;

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