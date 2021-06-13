using System;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.DomainExceptions
{
    public class DomainException : Exception
    {
        public DomainException()
        {
        }

        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}