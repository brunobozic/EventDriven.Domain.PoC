using System;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.UserUpdates.Notifications;

public class UserUpdatedPrimaryPhoneNotification : IntegrationEventBase<UserUpdatedPrimaryPhoneDomainEvent>
{
    public UserUpdatedPrimaryPhoneNotification(UserUpdatedPrimaryPhoneDomainEvent integrationEvent) : base(
        integrationEvent)
    {
        UserId = integrationEvent.UserId;
    }

    [JsonConstructor]
    public UserUpdatedPrimaryPhoneNotification(Guid userId) : base(null)
    {
        UserId = userId;
    }

    public Guid UserId { get; }
}