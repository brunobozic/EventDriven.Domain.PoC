using System;

namespace SharedKernel.DomainCoreInterfaces;

public interface IModificationAuditedEntity
{
    DateTimeOffset? DateModified { get; set; }
    Guid? ModifiedById { get; set; }
}