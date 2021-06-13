using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate
{
    public class Country : BasicDomainEntity<long>, IAuditTrail
    {
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}