using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;
using System;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.CUD.Notifications
{
    public class UserUndeletedNotification : IntegrationEventBase<UserUndeletedDomainEvent>
    {
        public UserUndeletedNotification(UserUndeletedDomainEvent integrationEvent) : base(integrationEvent)
        {
            UserId = integrationEvent.UserId;
        }

        [JsonConstructor]
        public UserUndeletedNotification(Guid userId) : base(null)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}