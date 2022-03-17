using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;
using System;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.UserUpdates.Notifications
{
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
}