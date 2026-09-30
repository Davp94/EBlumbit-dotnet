using System;

namespace EBlumbit.exceptions;

public abstract class DomainException : Exception
{
    public int StatusCode { get; }
    public string ErrorCode { get; }

    public DomainException(string message, int statusCode, string errorCode): base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}
