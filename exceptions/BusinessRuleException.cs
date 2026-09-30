using System;

namespace EBlumbit.exceptions;

public class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message) : base(message, 422, "BUSINESS_RULE_EXCEPTION")
    {
    }
}
