using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses
{
    public class DomainEventBase : IDomainEvent
    {
        public DomainEventBase()
        {
            OccurredOn = DateTime.Now;
            TypeOfEvent = EventTypeEnum.Undefined;
        }

        public DateTime OccurredOn { get; }
        public EventTypeEnum TypeOfEvent { get; set; }
    }
}