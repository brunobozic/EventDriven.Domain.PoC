using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.UserUpdates.Notifications;

public class UserUpdatedMailAddressNotification : DomainNotificationBase<UserUpdatedMailAddressDomainEvent>
{
    public UserUpdatedMailAddressNotification(UserUpdatedMailAddressDomainEvent integrationEvent, Guid id) : base(
        integrationEvent, id)
    {
        UserId = integrationEvent.UserId;
    }



    public Guid UserId { get; }
}