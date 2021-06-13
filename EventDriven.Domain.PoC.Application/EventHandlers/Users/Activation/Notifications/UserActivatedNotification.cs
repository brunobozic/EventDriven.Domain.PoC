using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.Activation.Notifications
{
    public class UserActivatedNotification : IntegrationEventBase<UserActivatedDomainEvent>
    {
        public UserActivatedNotification(UserActivatedDomainEvent integrationEvent) : base(integrationEvent)
        {
            UserId = integrationEvent.UserId;
        }

        [JsonConstructor]
        public UserActivatedNotification(UserId userId) : base(null)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}