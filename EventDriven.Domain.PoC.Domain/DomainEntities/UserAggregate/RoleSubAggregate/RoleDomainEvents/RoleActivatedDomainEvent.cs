using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents
{
    public class RoleActivatedDomainEvent : DomainEventBase
    {
        public long ActivatedByUserId;

        public string ActivationReason;
        public string ActivatorEmail;
        public long ActivatorId;
        public string ActivatorUsername;
        public DateTimeOffset DateActivated;
        public string Description;
        public string Name;
        public long RoleId;

        public RoleActivatedDomainEvent(long roleId, long activatedByUserId, string activatedByUserEmail,
            string activatedByUserUserName, string activationReason,
            DateTimeOffset dateActivated)
        {
            RoleId = roleId;
            ActivatedByUserId = activatedByUserId;
            ActivationReason = activationReason;
            DateActivated = dateActivated;
            ActivatorEmail = activatedByUserEmail;
            ActivatorUsername = activatedByUserUserName;
        }
    }
}