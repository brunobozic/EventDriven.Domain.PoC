﻿using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.CUD.Notifications
{
    public class UserDeletedNotification : IntegrationEventBase<UserDeletedDomainEvent>
    {
        public UserDeletedNotification(UserDeletedDomainEvent integrationEvent) : base(integrationEvent)
        {
            UserId = integrationEvent.UserId;
        }

        [JsonConstructor]
        public UserDeletedNotification(UserId userId) : base(null)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}