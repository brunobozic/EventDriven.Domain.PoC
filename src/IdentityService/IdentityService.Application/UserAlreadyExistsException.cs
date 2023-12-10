using System;
using System.Runtime.Serialization;

[Serializable]
public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string email, string userName)
        : base($"A user with email {email} or username {userName} already exists.")
    {
    }

    public UserAlreadyExistsException()
    {
    }

    public UserAlreadyExistsException(string message) : base(message)
    {
    }

    public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected UserAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}