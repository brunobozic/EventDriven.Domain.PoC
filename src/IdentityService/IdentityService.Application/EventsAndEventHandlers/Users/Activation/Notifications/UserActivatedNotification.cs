using System;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Activation.Notifications;

public class UserActivatedNotification : IntegrationEventBase<UserActivatedDomainEvent>
{
    public UserActivatedNotification(UserActivatedDomainEvent integrationEvent) : base(integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }

    [JsonConstructor]
    public UserActivatedNotification(Guid userId) : base(null)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}