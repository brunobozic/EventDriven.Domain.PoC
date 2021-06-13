using System;

namespace EventDriven.Domain.PoC.SharedKernel.DomainImplementations.DomainErrors
{
    public class ValueObjectIsInvalidException : Exception
    {
        public ValueObjectIsInvalidException(string message)
            : base(message)
        {
        }
    }
}