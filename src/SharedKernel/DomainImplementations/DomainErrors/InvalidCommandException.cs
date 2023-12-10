using System;

namespace SharedKernel.DomainImplementations.DomainErrors;

public class InvalidCommandException : Exception
{
    public InvalidCommandException(string message, string details) : base(message)
    {
        Details = details;
    }

    public string Details { get; }
}