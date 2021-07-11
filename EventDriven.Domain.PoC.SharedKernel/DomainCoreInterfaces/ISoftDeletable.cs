using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface ISoftDeletable
    {
        bool Deleted { get; set; }
        DateTimeOffset? DateDeleted { get; set; }
        Guid DeletedBy { get; set; }
    }
}