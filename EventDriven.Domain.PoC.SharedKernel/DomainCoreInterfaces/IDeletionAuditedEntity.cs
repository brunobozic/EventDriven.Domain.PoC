using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface IDeletionAuditedEntity
    {
        DateTimeOffset? DateDeleted { get; set; }
        Guid? DeletedById { get; set; }
    }
}