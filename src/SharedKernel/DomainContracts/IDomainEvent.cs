using System;
using MediatR;
using SharedKernel.DomainImplementations.BaseClasses;

namespace SharedKernel.DomainContracts;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
    EventTypeEnum TypeOfEvent { get; set; }
}