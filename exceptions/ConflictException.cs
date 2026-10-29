using System;

namespace EBlumbit.exceptions;

public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message, 409, "CONFLICT")
    {
    }
}
