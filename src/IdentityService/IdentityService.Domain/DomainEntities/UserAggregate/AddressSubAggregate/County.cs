using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedKernel.DomainCoreInterfaces;

namespace IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;

public class County : BasicDomainEntity<long>, IAuditTrail
{
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}