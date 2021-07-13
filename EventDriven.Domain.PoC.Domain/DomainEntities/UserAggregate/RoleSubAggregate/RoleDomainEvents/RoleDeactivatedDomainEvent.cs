using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents
{
    public class RoleDeactivatedDomainEvent : DomainEventBase
    {
        public DateTimeOffset DateDeactivated;
        public Guid DeactivatedById;

        public string DeactivationReason;
        public string DeactivatorEmail;
        public string DeactivatorUsername;
        public string Description;
        public string Name;
        public long RoleId;

        public RoleDeactivatedDomainEvent(long roleId, Guid deactivatedById, string deactivatedByUserEmail,
            string deactivatedByUserUserName, string deactivationReason,
            DateTimeOffset dateDeactivated)
        {
            RoleId = roleId;
            DeactivatedById = deactivatedById;
            DeactivationReason = deactivationReason;
            DateDeactivated = dateDeactivated;
            DeactivationReason = deactivationReason;
            DeactivatorEmail = deactivatedByUserEmail;
            DeactivatorUsername = deactivatedByUserUserName;
        }
    }
}