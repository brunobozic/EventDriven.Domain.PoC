using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedKernel.DomainCoreInterfaces;

namespace IdentityService.Domain.DomainEntities.UserAggregate.ContactsSubAggregate;

public class ContactType : BasicDomainEntity<long>, IAuditTrail
{
    public bool IsActive { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}