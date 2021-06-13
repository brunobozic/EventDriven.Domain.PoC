using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface ISoftDeletable
    {
        bool Deleted { get; set; }
        DateTimeOffset? DateDeleted { get; set; }
        long DeletedBy { get; set; }
    }
}