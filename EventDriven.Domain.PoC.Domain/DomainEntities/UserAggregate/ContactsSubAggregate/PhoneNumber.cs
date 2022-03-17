using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.ContactsSubAggregate
{
    public class PhoneNumber : BasicDomainEntity<long>, IAuditTrail
    {
        public bool IsActive { get; set; }

        public string AreaCode { get; set; }

        public string OperatorCode { get; set; }

        public string Number { get; set; }

        public virtual PhoneNumberType PhoneNumberType { get; set; }
        public bool? IsPrimary { get; set; } = true;

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}