using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;

public class EmailNotVerifiedNotification : IntegrationEventBase<EmailNotVerifiedDomainEvent>
{
    public string Email;

    public EmailNotVerifiedNotification(EmailNotVerifiedDomainEvent integrationEvent, Guid id) : base(integrationEvent)
    {
        Email = integrationEvent.Email;
    }


}