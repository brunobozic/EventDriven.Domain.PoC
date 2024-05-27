using Microsoft.Extensions.Logging;
using SharedKernel.DomainContracts;
using System;

namespace SharedKernel.DomainImplementations.BaseClasses;

public class DomainEventBase : IDomainEvent
{
    public DomainEventBase()
    {
        OccurredOn = DateTime.Now;
        TypeOfEvent = EventTypeEnum.Undefined;
        MessageId = Guid.NewGuid();
    }

    public DateTime OccurredOn { get; }
    public EventTypeEnum TypeOfEvent { get; set; }
    public Guid MessageId { get;  set; }
}