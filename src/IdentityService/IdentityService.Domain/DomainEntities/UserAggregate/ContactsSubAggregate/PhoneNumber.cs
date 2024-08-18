using IdentityService.Domain.DomainEntities.UserAggregate.ContactsSubAggregate;
using IdentityService.Domain.DomainEntities;
using SharedKernel.DomainCoreInterfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

public class PhoneNumber : BasicDomainEntity<long>, IAuditTrail
{
    #region Public Properties

    public bool IsActive { get; private set; }
    public string AreaCode { get; private set; }
    public string OperatorCode { get; private set; }
    public string Number { get; private set; }
    public virtual PhoneNumberType PhoneNumberType { get; private set; }
    public bool? IsPrimary { get; private set; } = true;

    #endregion Public Properties

    #region Public Methods

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods
}