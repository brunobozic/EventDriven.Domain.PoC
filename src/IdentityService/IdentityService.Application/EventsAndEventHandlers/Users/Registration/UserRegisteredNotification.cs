using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace IdentityService.Application.EventsAndEventHandlers.Users.Registration;

public class UserRegisteredNotification : IntegrationEventBase<UserRegisteredDomainEvent>
{
    public UserRegisteredNotification(UserRegisteredDomainEvent integrationEvent, Guid id) : base(integrationEvent)
    {
        UserId = integrationEvent.UserId;
        UserName = integrationEvent.UserName;
        UserEmail = integrationEvent.UserEmail;
    }



    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public Guid UserId { get; }
}