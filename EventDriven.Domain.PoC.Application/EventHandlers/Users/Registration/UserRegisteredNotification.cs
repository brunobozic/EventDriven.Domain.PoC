using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.Registration
{
    public class UserRegisteredNotification : IntegrationEventBase<UserRegisteredDomainEvent>
    {
        public UserRegisteredNotification(UserRegisteredDomainEvent integrationEvent) : base(integrationEvent)
        {
            UserId = integrationEvent.UserId;
        }

        [JsonConstructor]
        public UserRegisteredNotification(UserId userId) : base(null)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}