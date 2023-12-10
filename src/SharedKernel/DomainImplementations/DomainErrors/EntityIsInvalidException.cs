using System;

namespace SharedKernel.DomainImplementations.DomainErrors;

public class EntityIsInvalidException : Exception
{
    public EntityIsInvalidException(string message)
        : base(message)
    {
    }
}