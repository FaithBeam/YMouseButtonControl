using System;

namespace YMouseButtonControl.Core.Services.Profiles.Exceptions;

public class InvalidReplaceException : Exception
{
    public InvalidReplaceException() { }

    public InvalidReplaceException(string message)
        : base(message) { }

    public InvalidReplaceException(string message, Exception innerException)
        : base(message, innerException) { }
}
