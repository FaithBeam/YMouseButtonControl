using System;

namespace YMouseButtonControl.Profiles.Exceptions;

public class InvalidMoveException : Exception
{
    public InvalidMoveException() { }

    public InvalidMoveException(string message)
        : base(message) { }

    public InvalidMoveException(string message, Exception innerException)
        : base(message, innerException) { }
}
