using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain
{
    public class RoleId : TypedIdValueBase
    {
        public RoleId(Guid value) : base(value)
        {
        }
    }
}