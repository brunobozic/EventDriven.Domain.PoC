using System;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using Newtonsoft.Json;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Registration;

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