using System;

namespace SharedKernel.DomainCoreInterfaces;

public interface ICreationAuditedEntity
{
    Guid? CreatedById { get; set; }
    DateTimeOffset DateCreated { get; set; }
    bool IsDraft { get; set; }
    bool IsSeed { get; set; }
}