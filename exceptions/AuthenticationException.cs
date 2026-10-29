using System;

namespace EBlumbit.exceptions;

public class AuthenticationException : DomainException
{
    public AuthenticationException(string message) : base(message, 401, "AUTHENTICATION_EXCEPTION")
    {
    }
}
