using SharedKernel.DomainCoreInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;

public class CityBlock : BasicDomainEntity<long>, IAuditTrail
{
    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}