using System;

namespace Domain.Shared
{
    public class BusinessRuleValidationException : Exception
    {
        public BusinessRuleValidationException(string message)
            : base(message)
        { }
    }
}
