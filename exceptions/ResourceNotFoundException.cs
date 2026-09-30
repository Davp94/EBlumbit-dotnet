using System;

namespace EBlumbit.exceptions;

public class ResourceNotFoundException : DomainException
{
    public ResourceNotFoundException(string message) : base(message, 404, "NOT_FOUND_EXCEPTION")
    {
    }
}
