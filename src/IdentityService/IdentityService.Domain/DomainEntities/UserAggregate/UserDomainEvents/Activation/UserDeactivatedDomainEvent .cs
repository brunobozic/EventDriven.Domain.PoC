using System;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation;

public class UserDeactivatedDomainEvent : DomainEventBase
{
    public UserDeactivatedDomainEvent(
        string email
        , string userName
        , Guid userId
        , string oib
        , Guid deactivatedById
        , string deactivationReason
    )
    {
        Email = email;
        UserName = userName;
        UserId = userId;
        Oib = oib;
        DeactivatedById = deactivatedById;
        DeactivationReason = deactivationReason;
        DeactivationDateStamp = DateTime.UtcNow;
    }

    public User DeactivatedBy { get; set; }
    public string DeactivationReason { get; set; }
    public DateTime DeactivationDateStamp { get; set; }
    public Guid? DeactivatedById { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Oib { get; set; }
    public Guid UserId { get; set; }
}