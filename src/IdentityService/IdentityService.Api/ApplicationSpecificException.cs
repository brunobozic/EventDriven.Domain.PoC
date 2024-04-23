using System;
using System.Runtime.Serialization;

namespace IdentityService.Api;
[Serializable]
public sealed class ApplicationSpecificException : Exception
{
    public ApplicationSpecificException()
    {
    }

    public ApplicationSpecificException(string message) : base(message)
    {
    }

    private ApplicationSpecificException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}