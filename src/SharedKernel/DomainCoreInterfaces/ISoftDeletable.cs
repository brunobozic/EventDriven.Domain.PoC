using System;

namespace SharedKernel.DomainCoreInterfaces;

public interface ISoftDeletable
{
    DateTimeOffset? DateDeleted { get; set; }
    bool Deleted { get; set; }
    Guid DeletedBy { get; set; }
}