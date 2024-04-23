using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.UserUpdates.Notifications;

public class UserUpdatedPrimaryPhoneNotification : IntegrationEventBase<UserUpdatedPrimaryPhoneDomainEvent>
{
    public UserUpdatedPrimaryPhoneNotification(UserUpdatedPrimaryPhoneDomainEvent integrationEvent, Guid id) : base(
        integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }



    public Guid UserId { get; }
}