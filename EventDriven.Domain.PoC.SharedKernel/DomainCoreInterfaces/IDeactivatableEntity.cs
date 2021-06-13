using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface IDeactivatableEntity
    {
        bool IsActive { get; set; }
        DateTimeOffset ActiveFrom { get; set; }
        DateTimeOffset? ActiveTo { get; set; }
    }
}