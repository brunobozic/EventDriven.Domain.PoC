using System;

namespace SharedKernel.DomainCoreInterfaces;

public interface IDeletionAuditedEntity
{
    DateTimeOffset? DateDeleted { get; set; }
    Guid? DeletedById { get; set; }
}