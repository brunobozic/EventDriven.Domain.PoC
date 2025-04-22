using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.CUD.Notifications;

public class UserCreatedNotification : IntegrationEventBase<UserCreatedDomainEvent>
{
    public string ActivationLink;
    public DateTimeOffset? ActivationLinkGenerated;
    public string Email;
    public string FirstName;
    public string LastName;
    public string Origin;
    public Guid ResourceId;
    public Guid MessageId;
    public string UserName;

    public UserCreatedNotification(UserCreatedDomainEvent integrationEvent) : base(integrationEvent)
    {
        ActivationLink = integrationEvent.ActivationLink;
        FirstName = integrationEvent.FirstName;
        LastName = integrationEvent.LastName;
        Email = integrationEvent.Email;
        UserName = integrationEvent.UserName;
        ActivationLinkGenerated = integrationEvent.ActivationLinkGenerated;
        Origin = integrationEvent.Origin;
        ResourceId = integrationEvent.UserResourceId;
        MessageId = integrationEvent.MessageId;
    }

    [JsonConstructor]
    public UserCreatedNotification(
       string activationLink,
       string firstName,
       string lastName,
       string email,
       string userName,
       DateTimeOffset? activationLinkGenerated,
       string origin,
       Guid resourceId,
       Guid messageId
   ) : base(new UserCreatedDomainEvent(resourceId, email, userName, firstName, lastName, "", DateTimeOffset.MinValue, null, "", null, resourceId, origin, EventTypeEnum.UserCreatedDomainEvent))
    {
        ActivationLink = activationLink;
        FirstName = firstName;
        LastName = lastName;
        ActivationLinkGenerated = activationLinkGenerated;
        ActivationLink = activationLink;
        Email = email;
        UserName = userName;
        Origin = origin;
        ResourceId = resourceId;
        MessageId = messageId;
    }
}