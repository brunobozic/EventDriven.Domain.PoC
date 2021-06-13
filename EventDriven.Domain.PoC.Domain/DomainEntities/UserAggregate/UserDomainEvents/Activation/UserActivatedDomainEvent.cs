using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.Activation
{
    public class UserActivatedDomainEvent : DomainEventBase
    {
        public UserActivatedDomainEvent(
            string email
            , string userName
            , Guid userId
            , string oib
            , long activatedByUserId
            , string activationReason
            , long activatedById
        )
        {
            Email = email;
            UserName = userName;
            UserId = new UserId(userId);
            Oib = oib;
            ActivatedById = activatedByUserId;
            ActivationReason = activationReason;
            ActivationDateStamp = DateTime.UtcNow;
            ActivatedById = activatedById;
        }

        public long ActivatedById { get; set; }

        public string ActivationReason { get; set; }
        public DateTime ActivationDateStamp { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Oib { get; set; }
        public UserId UserId { get; set; }
    }
}