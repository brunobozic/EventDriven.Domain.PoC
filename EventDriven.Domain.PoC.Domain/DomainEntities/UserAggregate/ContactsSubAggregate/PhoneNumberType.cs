﻿using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.ContactsSubAggregate
{
    public class PhoneNumberType : BasicDomainEntity<long>, IAuditTrail
    {
        public bool IsActive { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}