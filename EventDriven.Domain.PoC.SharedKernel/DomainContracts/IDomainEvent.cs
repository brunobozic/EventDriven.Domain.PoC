using System;
using MediatR;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
    }
}