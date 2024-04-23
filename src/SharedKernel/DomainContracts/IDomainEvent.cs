using MediatR;
using SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace SharedKernel.DomainContracts;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
    EventTypeEnum TypeOfEvent { get; set; }
    
}