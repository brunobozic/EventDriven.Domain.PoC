using System;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;

namespace EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses
{
    public class DomainEventBase : IDomainEvent
    {
        public DomainEventBase()
        {
            OccurredOn = DateTime.Now;
        }

        public DateTime OccurredOn { get; }
    }
}