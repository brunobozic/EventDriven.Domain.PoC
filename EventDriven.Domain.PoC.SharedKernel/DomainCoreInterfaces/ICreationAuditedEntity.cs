using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface ICreationAuditedEntity
    {
        Guid? CreatedById { get; set; }
        DateTimeOffset DateCreated { get; set; }
        bool IsDraft { get; set; }
        bool IsSeed { get; set; }
    }
}