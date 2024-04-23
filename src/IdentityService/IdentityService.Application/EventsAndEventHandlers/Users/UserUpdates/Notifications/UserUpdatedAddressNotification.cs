using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.UserUpdates.Notifications;

public class UserUpdatedAddressNotification : DomainNotificationBase<UserUpdatedAddressDomainEvent>
{
    public UserUpdatedAddressNotification(UserUpdatedAddressDomainEvent integrationEvent, Guid id) : base(integrationEvent, id)
    {
        UserId = integrationEvent.UserId;
    }



    public Guid UserId { get; }
}