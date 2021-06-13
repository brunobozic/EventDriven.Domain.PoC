using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents
{
    public class RoleDeactivatedDomainEvent : DomainEventBase
    {
        public DateTimeOffset DateDeactivated;
        public long DeactivatedByUserId;

        public string DeactivationReason;
        public string DeactivatorEmail;
        public string DeactivatorUsername;
        public string Description;
        public string Name;
        public long RoleId;

        public RoleDeactivatedDomainEvent(long roleId, long deactivatedByUserId, string deactivatedByUserEmail,
            string deactivatedByUserUserName, string deactivationReason,
            DateTimeOffset dateDeactivated)
        {
            RoleId = roleId;
            DeactivatedByUserId = deactivatedByUserId;
            DeactivationReason = deactivationReason;
            DateDeactivated = dateDeactivated;
            DeactivationReason = deactivationReason;
            DeactivatorEmail = deactivatedByUserEmail;
            DeactivatorUsername = deactivatedByUserUserName;
        }
    }
}