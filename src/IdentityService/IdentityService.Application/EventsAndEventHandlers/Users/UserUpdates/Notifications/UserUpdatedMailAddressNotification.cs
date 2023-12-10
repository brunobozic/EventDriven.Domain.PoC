using System;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.UserUpdates.Notifications;

public class UserUpdatedMailAddressNotification : IntegrationEventBase<UserUpdatedMailAddressDomainEvent>
{
    public UserUpdatedMailAddressNotification(UserUpdatedMailAddressDomainEvent integrationEvent) : base(
        integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }

    [JsonConstructor]
    public UserUpdatedMailAddressNotification(Guid userId) : base(null)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}