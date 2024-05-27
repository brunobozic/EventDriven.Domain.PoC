using IdentityService.Domain.DomainEntities.UserAggregate.ContactsSubAggregate;
using IdentityService.Domain.DomainEntities;
using SharedKernel.DomainCoreInterfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

public class EmailAddress : BasicDomainEntity<long>, IAuditTrail
{
    #region Public Properties

    public bool IsActive { get; private set; }
    public string Email { get; private set; }
    public bool? IsPrimary { get; private set; } = true;
    public bool? IsConfirmed { get; private set; } = false;
    public virtual EmailType EmailType { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods
}
