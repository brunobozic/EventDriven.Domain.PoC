using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface IExecutionContextAccessor
    {
        Guid CorrelationId { get; }

        bool IsAvailable { get; }
    }
}