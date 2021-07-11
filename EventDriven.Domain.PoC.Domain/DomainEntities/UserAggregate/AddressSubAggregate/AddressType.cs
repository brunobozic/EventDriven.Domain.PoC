using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.Domain.DomainEntities.DomainExceptions;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate
{
    public class AddressType : BasicDomainEntity<long>, IAuditTrail
    {
        #region Public properties

        public Guid ReactivatedById { get; set; }
        public Guid DeactivatedById { get; set; }
        public Guid UndeletedById { get; set; }

        #endregion Public properties

        #region ctor

        public static AddressType NewDraft(
            string name
            , string description
            , User creatorUser
            , DateTimeOffset dateCreated
        )
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            var addressType = new AddressType
            {
                Name = name.Trim(),
                Description = description.Trim()
            };

            return addressType;
        }

        public static AddressType NewActiveDraft(
            string name
            , string description
            , User creatorUser
            , DateTimeOffset dateCreated
            , DateTimeOffset activeFrom
            , DateTimeOffset activeTo
        )
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            var addressType = new AddressType
            {
                Name = name.Trim(),
                Description = description.Trim()
            };

            addressType.Activate(activeFrom, activeTo, creatorUser);
            addressType.AssignCreatedBy(creatorUser);

            return addressType;
        }

        #endregion ctor

        #region Public methods

        public void ChangeDescription(string newDescription)
        {
            if (EnsureIsActive() && !IsDeleted() && !IsDeactivated())
                Description = newDescription;
            else
                throw new DomainException(
                    "The address type is either deactivated, deleted or has expired, unable to change its description.");
        }

        public bool EnsureIsActive()
        {
            return ActiveTo >= DateTimeOffset.UtcNow;
        }

        public virtual bool IsDeleted()
        {
            return Deleted;
        }

        public virtual bool IsDeactivated()
        {
            return !Active;
        }

        public virtual bool IsExpired(DateTimeOffset theDate)
        {
            return ActiveTo < theDate;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        #endregion Public methods
    }
}