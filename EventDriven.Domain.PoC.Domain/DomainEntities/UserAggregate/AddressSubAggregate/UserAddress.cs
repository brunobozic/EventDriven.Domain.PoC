using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate
{
    public class UserAddress : BasicDomainEntity<long>, IAuditTrail
    {
        #region Public Properties

        public Guid UserRoleGuid { get; private set; }

        #endregion Public Properties

        #region Navigation Properties

        public Address Address { get; private set; }
        public User User { get; private set; }

        #endregion Navigation Properties

        #region FK

        public Guid UserId { get; private set; }
        public long AddressId { get; private set; }
        public Guid? UndeletedById { get; private set; }
        public Guid? DeactivatedById { get; private set; }
        public Guid? ReactivatedById { get; private set; }

        #endregion FK

        #region ctor

        public static UserAddress NewDraft(User applicationUser, Address address, User creator)
        {
            var userAddress = new UserAddress
                {User = applicationUser, Address = address, UserRoleGuid = Guid.NewGuid()};
            userAddress.AssignCreatedBy(creator);

            // activated by the creator
            userAddress.Activate(DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddYears(Consts.DEFAULT_ACTIVETO_VALUE_FOR_USERADDRESS), creator);

            return userAddress;
        }

        public static UserAddress NewActivatedDraft(User applicationUser, Address address, User creator)
        {
            var userAddress = new UserAddress
                {User = applicationUser, Address = address, UserRoleGuid = Guid.NewGuid()};
            userAddress.AssignCreatedBy(creator);

            // activated by the creator
            userAddress.Activate(DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddYears(Consts.DEFAULT_ACTIVETO_VALUE_FOR_USERADDRESS), creator);
            userAddress.ActivatedById = creator.Id;

            return userAddress;
        }

        public static UserAddress NewInactiveDraft(User applicationUser, Address address, User creator)
        {
            var userAddress = new UserAddress
                {User = applicationUser, Address = address, UserRoleGuid = Guid.NewGuid()};

            // not activated, but still has a creator
            userAddress.AssignCreatedBy(creator);

            return userAddress;
        }

        #endregion ctor

        #region Public Methods

        private bool EnsureIsActive()
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

        #endregion Public Methods
    }
}