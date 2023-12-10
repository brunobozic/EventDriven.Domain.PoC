using System;
using System.Globalization;

namespace SharedKernel.DomainImplementations.DomainErrors;

// custom exception class for throwing application specific exceptions
// that can be caught and handled within the application
public class AppException : Exception
{
    public AppException()
    {
    }

    public AppException(string message) : base(message)
    {
    }

    public AppException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}