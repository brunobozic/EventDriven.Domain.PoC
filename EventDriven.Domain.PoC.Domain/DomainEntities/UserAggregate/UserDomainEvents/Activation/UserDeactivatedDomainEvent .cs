using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation
{
    public class UserDeactivatedDomainEvent : DomainEventBase
    {
        public UserDeactivatedDomainEvent(
            string email
            , string userName
            , Guid userId
            , string oib
            , long deactivatedByUserId
            , string deactivationReason
            , long deactivatedById
        )
        {
            Email = email;
            UserName = userName;
            UserId = new UserId(userId);
            Oib = oib;
            DeactivatedById = deactivatedByUserId;
            DeactivationReason = deactivationReason;
            DeactivationDateStamp = DateTime.UtcNow;
            DeactivatedById = deactivatedById;
        }

        public User DeactivatedBy { get; set; }

        public string DeactivationReason { get; set; }
        public DateTime DeactivationDateStamp { get; set; }
        public long? DeactivatedById { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Oib { get; set; }
        public UserId UserId { get; set; }
    }
}