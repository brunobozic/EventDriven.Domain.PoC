using MediatR;
using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainContracts
{
    public interface IDomainEvent : INotification
    {
        DateTime OccurredOn { get; }
        EventTypeEnum TypeOfEvent { get; set; }
    }
}