using System;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.CUD.Notifications;

public class UserDeletedNotification : IntegrationEventBase<UserDeletedDomainEvent>
{
    public UserDeletedNotification(UserDeletedDomainEvent integrationEvent) : base(integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }

    [JsonConstructor]
    public UserDeletedNotification(Guid userId) : base(null)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}