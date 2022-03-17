﻿using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;
using System;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.Activation.Notifications
{
    public class UserReactivatedNotification : IntegrationEventBase<UserReactivatedDomainEvent>
    {
        public UserReactivatedNotification(UserReactivatedDomainEvent integrationEvent) : base(integrationEvent)
        {
            UserId = integrationEvent.UserId;
        }

        [JsonConstructor]
        public UserReactivatedNotification(Guid userId) : base(null)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}