﻿using System;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.CUD.Notifications
{
    public class UserCreatedNotification : IntegrationEventBase<UserCreatedDomainEvent>
    {
        public string ActivationLink;
        public DateTimeOffset? ActivationLinkGenerated;
        public string Email;
        public string FirstName;
        public string LastName;
        public string Origin;
        public Guid UserId;

        public UserCreatedNotification(UserCreatedDomainEvent integrationEvent) : base(integrationEvent)
        {
            UserId = integrationEvent.UserId;
            ActivationLink = integrationEvent.ActivationLink;
            FirstName = integrationEvent.FirstName;
            LastName = integrationEvent.LastName;
            Email = integrationEvent.Email;
            ActivationLinkGenerated = integrationEvent.ActivationLinkGenerated;
            Origin = integrationEvent.Origin;
        }

        [JsonConstructor]
        public UserCreatedNotification(
            Guid userId,
            string activationLink,
            string firstName,
            string lastName,
            string email,
            DateTimeOffset? activationLinkGenerated,
            string Origin
        ) : base(null)
        {
            UserId = userId;
            ActivationLink = activationLink;
            FirstName = firstName;
            LastName = lastName;
            ActivationLinkGenerated = activationLinkGenerated;
            ActivationLink = activationLink;
            Email = email;
            Origin = this.Origin;
        }
    }
}