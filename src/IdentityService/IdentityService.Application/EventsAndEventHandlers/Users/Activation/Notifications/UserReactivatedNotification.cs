using System;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Activation.Notifications;

public class UserReactivatedNotification : IntegrationEventBase<UserReactivatedDomainEvent>
{
    public UserReactivatedNotification(UserReactivatedDomainEvent integrationEvent) : base(integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }

    [JsonConstructor]
    public UserReactivatedNotification(Guid userId) : base(null)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}