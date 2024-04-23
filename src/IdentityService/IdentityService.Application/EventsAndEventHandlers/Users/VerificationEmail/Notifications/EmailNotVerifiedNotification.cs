using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;

public class EmailNotVerifiedNotification : DomainNotificationBase<EmailNotVerifiedDomainEvent>
{
    public string Email;

    public EmailNotVerifiedNotification(EmailNotVerifiedDomainEvent integrationEvent, Guid id) : base(integrationEvent, id)
    {
        Email = integrationEvent.Email;
    }


}