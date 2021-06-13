using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain
{
    public class AddressId : TypedIdValueBase
    {
        public AddressId(Guid value) : base(value)
        {
        }
    }
}