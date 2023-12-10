using System;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

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
    public Guid UserId;
    public string UserName;

    public UserCreatedNotification(UserCreatedDomainEvent integrationEvent) : base(integrationEvent)
    {
        UserId = integrationEvent.UserId;
        ActivationLink = integrationEvent.ActivationLink;
        FirstName = integrationEvent.FirstName;
        LastName = integrationEvent.LastName;
        Email = integrationEvent.Email;
        ActivationLinkGenerated = integrationEvent.ActivationLinkGenerated;
        Origin = integrationEvent.Origin;
        ResourceId = integrationEvent.UserResourceId;
    }

    [JsonConstructor]
    public UserCreatedNotification(
        Guid userId,
        string activationLink,
        string firstName,
        string lastName,
        string email,
        string userName,
        DateTimeOffset? activationLinkGenerated,
        Guid resourceId,
        string Origin
    ) : base(null)
    {
        UserId = userId;
        ActivationLink = activationLink;
        FirstName = firstName;
        LastName = lastName;
        ActivationLinkGenerated = activationLinkGenerated;
        ActivationLink = activationLink;
        Email = email;
        UserName = userName;
        Origin = this.Origin;
        ResourceId = resourceId;
    }
}