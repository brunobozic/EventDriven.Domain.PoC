using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainImplementations.DomainErrors
{
    public class EntityIsInvalidException : Exception
    {
        public EntityIsInvalidException(string message)
            : base(message)
        {
        }
    }
}