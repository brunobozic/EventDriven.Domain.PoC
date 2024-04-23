using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Activation.Notifications;

public class UserActivatedNotification : DomainNotificationBase<UserActivatedDomainEvent>
{
    public UserActivatedNotification(UserActivatedDomainEvent integrationEvent, Guid id) : base(integrationEvent, id)
    {
        UserId = integrationEvent.UserId;
    }



    public Guid UserId { get; }
}