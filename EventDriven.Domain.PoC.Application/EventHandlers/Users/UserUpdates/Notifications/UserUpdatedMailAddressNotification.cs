using EventDriven.Domain.PoC.Domain;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.EventHandlers.Users.UserUpdates.Notifications
{
    public class UserUpdatedMailAddressNotification : IntegrationEventBase<UserUpdatedMailAddressDomainEvent>
    {
        public UserUpdatedMailAddressNotification(UserUpdatedMailAddressDomainEvent integrationEvent) : base(
            integrationEvent)
        {
            UserId = integrationEvent.UserId;
        }

        [JsonConstructor]
        public UserUpdatedMailAddressNotification(UserId userId) : base(null)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}