using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.CUD.Notifications;

public class UserCreatedNotification : DomainNotificationBase<UserCreatedDomainEvent>
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

    [JsonConstructor]
    public UserCreatedNotification(UserCreatedDomainEvent integrationEvent, Guid id) : base(integrationEvent, id)
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
}