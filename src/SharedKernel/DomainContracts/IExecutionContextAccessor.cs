using System;

namespace SharedKernel.DomainContracts;

public interface IExecutionContextAccessor
{
    Guid CorrelationId { get; }

    bool IsAvailable { get; }
}