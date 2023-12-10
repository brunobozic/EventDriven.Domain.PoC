using System;

namespace SharedKernel.DomainImplementations.DomainErrors;

public class ValueObjectIsInvalidException : Exception
{
    public ValueObjectIsInvalidException(string message)
        : base(message)
    {
    }
}