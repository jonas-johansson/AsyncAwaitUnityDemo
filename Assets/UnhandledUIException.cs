using System;

public class UnhandledUIException : Exception
{
    public UnhandledUIException()
    {
    }

    public UnhandledUIException(string message) : base(message)
    {
    }

    public UnhandledUIException(string message, Exception inner) : base(message, inner)
    {
    }
}