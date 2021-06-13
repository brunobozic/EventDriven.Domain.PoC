using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation
{
    public class UserReactivatedDomainEvent : DomainEventBase
    {
        public UserReactivatedDomainEvent(
            string email
            , string userName
            , Guid userId
            , string oib
            , long reactivatedByUserId
            , string reactivationReason
            , long reactivatedById
        )
        {
            Email = email;
            UserName = userName;
            UserId = new UserId(userId);
            Oib = oib;
            ReactivatedById = reactivatedByUserId;
            ReactivationReason = reactivationReason;
            ReactivationDateStamp = DateTime.UtcNow;
        }

        public long ReactivatedById { get; set; }

        public string ReactivationReason { get; set; }
        public DateTime ReactivationDateStamp { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Oib { get; set; }
        public UserId UserId { get; set; }
    }
}