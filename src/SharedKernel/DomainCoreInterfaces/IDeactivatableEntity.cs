using System;

namespace SharedKernel.DomainCoreInterfaces;

public interface IDeactivatableEntity
{
    DateTimeOffset ActiveFrom { get; set; }
    DateTimeOffset? ActiveTo { get; set; }
    bool IsActive { get; set; }
}