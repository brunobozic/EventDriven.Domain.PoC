using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using SharedKernel.DomainCoreInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.DomainEntities.UserAggregate.ContactsSubAggregate;

public class Contact : BasicDomainEntity<long>, IAuditTrail
{
    #region Public Properties

    public bool IsActive { get; private set; }

    private readonly List<Address> _addresses = new();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    private readonly List<EmailAddress> _emails = new();
    public IReadOnlyCollection<EmailAddress> Emails => _emails.AsReadOnly();

    private readonly List<PhoneNumber> _phones = new();
    public IReadOnlyCollection<PhoneNumber> Phones => _phones.AsReadOnly();

    public virtual ContactType ContactType { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods
}
