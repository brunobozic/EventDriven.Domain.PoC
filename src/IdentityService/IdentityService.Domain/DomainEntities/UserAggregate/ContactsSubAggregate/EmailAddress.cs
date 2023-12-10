using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedKernel.DomainCoreInterfaces;

namespace IdentityService.Domain.DomainEntities.UserAggregate.ContactsSubAggregate;

public class EmailAddress : BasicDomainEntity<long>, IAuditTrail
{
    public bool IsActive { get; set; }

    public string Email { get; set; }

    public bool? IsPrimary { get; set; } = true;
    public bool? IsConfirmed { get; set; } = false;

    public virtual EmailType EmailType { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}