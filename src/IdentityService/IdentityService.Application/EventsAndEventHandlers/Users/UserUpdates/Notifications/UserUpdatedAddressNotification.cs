using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.UserUpdates.Notifications;

public class UserUpdatedAddressNotification : IntegrationEventBase<UserUpdatedAddressDomainEvent>
{
    public UserUpdatedAddressNotification(UserUpdatedAddressDomainEvent integrationEvent, Guid id) : base(integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }



    public Guid UserId { get; }
}