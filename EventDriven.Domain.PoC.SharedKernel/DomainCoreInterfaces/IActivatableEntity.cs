using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces
{
    public interface IActivatableEntity
    {
        DateTimeOffset ActiveFrom { get; set; }
        DateTimeOffset ActiveTo { get; set; }
        bool IsActive { get; set; }
    }
}