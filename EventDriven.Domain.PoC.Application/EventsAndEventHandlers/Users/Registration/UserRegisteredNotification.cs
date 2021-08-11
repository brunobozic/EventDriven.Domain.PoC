using System;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.Registration
{
    public class UserRegisteredNotification : IntegrationEventBase<UserRegisteredDomainEvent>
    {
        public UserRegisteredNotification(UserRegisteredDomainEvent integrationEvent) : base(integrationEvent)
        {
            UserId = integrationEvent.UserId;
            UserName = integrationEvent.UserName;
            UserEmail = integrationEvent.UserEmail;
        }

        [JsonConstructor]
        public UserRegisteredNotification(Guid userId, string userName, string userEmail) : base(null)
        {
            UserId = userId;
            UserName = userName;
            UserEmail = userEmail;
        }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public Guid UserId { get; }
    }
}