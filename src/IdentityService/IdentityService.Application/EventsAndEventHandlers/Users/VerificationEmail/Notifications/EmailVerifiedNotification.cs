using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;

public class EmailVerifiedNotification : DomainNotificationBase<EmailVerifiedDomainEvent>
{
    public string Email;

    public EmailVerifiedNotification(EmailVerifiedDomainEvent integrationEvent, Guid id) : base(integrationEvent, id)
    {
        Email = integrationEvent.Email;
    }

}