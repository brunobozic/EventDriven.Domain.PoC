using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface ISoftDeletable
    {
        DateTimeOffset? DateDeleted { get; set; }
        bool Deleted { get; set; }
        Guid DeletedBy { get; set; }
    }
}