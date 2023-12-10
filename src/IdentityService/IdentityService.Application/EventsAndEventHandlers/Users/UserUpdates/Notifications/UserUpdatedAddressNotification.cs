using System;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.UserUpdates.Notifications;

public class UserUpdatedAddressNotification : IntegrationEventBase<UserUpdatedAddressDomainEvent>
{
    public UserUpdatedAddressNotification(UserUpdatedAddressDomainEvent integrationEvent) : base(integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }

    [JsonConstructor]
    public UserUpdatedAddressNotification(Guid userId) : base(null)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}