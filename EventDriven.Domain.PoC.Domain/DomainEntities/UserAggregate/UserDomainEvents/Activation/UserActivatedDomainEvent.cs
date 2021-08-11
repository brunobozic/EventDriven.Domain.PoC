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
            , Guid activatedById
            , string activationReason
        )
        {
            Email = email;
            UserName = userName;
            UserId = userId;
            Oib = oib;
            ActivatedById = activatedById;
            ActivationReason = activationReason;
            ActivationDateStamp = DateTime.UtcNow;
        }

        public Guid? ActivatedById { get; set; }
        public string ActivationReason { get; set; }
        public DateTime ActivationDateStamp { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Oib { get; set; }
        public Guid UserId { get; set; }
    }
}