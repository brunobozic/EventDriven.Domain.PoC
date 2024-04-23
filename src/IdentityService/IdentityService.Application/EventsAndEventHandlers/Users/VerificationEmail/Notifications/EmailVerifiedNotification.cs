using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;

public class EmailVerifiedNotification : IntegrationEventBase<EmailVerifiedDomainEvent>
{
    public string Email;

    public EmailVerifiedNotification(EmailVerifiedDomainEvent integrationEvent, Guid id) : base(integrationEvent)
    {
        Email = integrationEvent.Email;
    }

}