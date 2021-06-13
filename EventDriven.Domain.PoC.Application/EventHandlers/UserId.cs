using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Application.EventHandlers
{
    public class UserID : TypedIdValueBase
    {
        public UserID(Guid value) : base(value)
        {
        }
    }
}