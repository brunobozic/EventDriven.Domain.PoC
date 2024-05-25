using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.CUD.Notifications;

public class UserDeletedNotification : IntegrationEventBase<UserDeletedDomainEvent>
{
    public UserDeletedNotification(UserDeletedDomainEvent integrationEvent, Guid id) : base(integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }



    public Guid UserId { get; }
}