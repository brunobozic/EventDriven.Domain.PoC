using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface IModificationAuditedEntity
    {
        DateTimeOffset? DateModified { get; set; }
        long? ModifiedById { get; set; }
    }
}