using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.Activation.Notifications
{
    public class UserDeactivatedNotification : IntegrationEventBase<UserDeactivatedDomainEvent>
    {
        public UserDeactivatedNotification(UserDeactivatedDomainEvent integrationEvent) : base(integrationEvent)
        {
            UserId = integrationEvent.UserId;
        }

        [JsonConstructor]
        public UserDeactivatedNotification(UserId userId) : base(null)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}