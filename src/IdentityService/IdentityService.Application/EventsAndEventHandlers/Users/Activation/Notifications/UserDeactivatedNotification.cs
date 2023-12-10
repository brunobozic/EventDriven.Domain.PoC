using System;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Activation.Notifications;

public class UserDeactivatedNotification : IntegrationEventBase<UserDeactivatedDomainEvent>
{
    public UserDeactivatedNotification(UserDeactivatedDomainEvent integrationEvent) : base(integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }

    [JsonConstructor]
    public UserDeactivatedNotification(Guid userId) : base(null)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}