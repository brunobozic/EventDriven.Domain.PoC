using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Verification;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.VerificationEmail.Notifications;

public class EmailVerifiedNotification : IntegrationEventBase<EmailVerifiedDomainEvent>
{
    public string UserName;
    public string Email;
    public string Origin;
    public DateTimeOffset? VerifiedDate;
    public Guid UserResourceId;
    public Guid MessageId;

    public EmailVerifiedNotification(EmailVerifiedDomainEvent integrationEvent) : base(integrationEvent)
    {
        Origin = integrationEvent.Origin;
        UserName = integrationEvent.UserName;
        Email = integrationEvent.Email;
        VerifiedDate = integrationEvent.VerifiedDate;
        UserResourceId = integrationEvent.UserResourceId;
        MessageId = integrationEvent.MessageId;
    }

    [JsonConstructor]
    public EmailVerifiedNotification(
       string email,
       string userName,
       Guid resourceId,
       string origin,
       Guid messageId
   ) : base(new EmailVerifiedDomainEvent(email, userName, resourceId, origin, EventTypeEnum.VerificationEmailAcknowledged))
    {
        Email = email;
        Origin = origin;
        UserName = userName;
        Email = email;
        EventType = EventTypeEnum.VerificationEmailAcknowledged;
        VerifiedDate = DateTimeOffset.UtcNow;
        UserResourceId = resourceId;
        MessageId = messageId;
    }
}