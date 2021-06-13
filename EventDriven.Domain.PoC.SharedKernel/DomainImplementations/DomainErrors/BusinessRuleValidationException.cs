using System;
using System.Runtime.Serialization;
using EventDriven.Domain.PoC.SharedKernel.BusinessRules;

namespace EventDriven.Domain.PoC.SharedKernel.DomainImplementations.DomainErrors
{
    [Serializable]
    public class BusinessRuleValidationException : Exception
    {
        private IBusinessRule rule;

        public BusinessRuleValidationException()
        {
        }

        public BusinessRuleValidationException(IBusinessRule rule)
        {
            this.rule = rule;
        }

        public BusinessRuleValidationException(string message) : base(message)
        {
        }

        public BusinessRuleValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BusinessRuleValidationException(SerializationInfo info, StreamingContext context) : base(info,
            context)
        {
        }
    }
}