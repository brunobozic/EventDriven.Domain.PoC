using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.ContactsSubAggregate
{
    public class Contact : BasicDomainEntity<long>, IAuditTrail
    {
        public bool IsActive { get; set; }

        public virtual ICollection<Address> Address { get; set; } = new HashSet<Address>();
        public virtual ContactType ContactType { get; set; }
        public virtual ICollection<EmailAddress> Email { get; set; } = new HashSet<EmailAddress>();
        public virtual ICollection<PhoneNumber> Phone { get; set; } = new HashSet<PhoneNumber>();

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}