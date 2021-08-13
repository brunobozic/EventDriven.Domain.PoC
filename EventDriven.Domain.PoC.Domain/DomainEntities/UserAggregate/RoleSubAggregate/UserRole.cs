using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate
{
    public class UserRole : BasicDomainEntity<long>, IAuditTrail
    {
        #region Public Properties

        public Guid UserRoleGuid { get; private set; }

        #endregion Public Properties

        #region Navigation Properties

        public Role Role { get; private set; }
        public User User { get; private set; }

        #endregion Navigation Properties

        #region FK

        public Guid UserId { get; private set; }
        public long RoleId { get; private set; }
        public Guid? UndeletedById { get; private set; }
        public Guid? DeactivatedById { get; private set; }
        public Guid? ReactivatedById { get; private set; }

        #endregion FK

        #region ctor

        public static UserRole NewDraft(User applicationUser, Role applicationRole, User activator)
        {
            var userRole = new UserRole {User = applicationUser, Role = applicationRole, UserRoleGuid = Guid.NewGuid()};
            userRole.AssignCreatedBy(activator);

            // activated by the creator
            userRole.Activate(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(1), activator);

            return userRole;
        }

        public static UserRole NewActivatedDraft(User applicationUser, Role applicationRole, User activator)
        {
            var userRole = new UserRole {User = applicationUser, Role = applicationRole, UserRoleGuid = Guid.NewGuid()};
            userRole.AssignCreatedBy(activator);

            // activated by the creator
            userRole.Activate(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(1), activator);
            userRole.ActivatedById = activator.Id;

            return userRole;
        }

        public static UserRole NewInactiveDraft(User applicationUser, Role applicationRole, User activator)
        {
            var userRole = new UserRole {User = applicationUser, Role = applicationRole, UserRoleGuid = Guid.NewGuid()};

            // not activated, but still has a creator
            userRole.AssignCreatedBy(activator);

            return userRole;
        }

        #endregion ctor

        #region Public Methods

        private bool EnsureIsActive()
        {
            return ActiveTo >= DateTimeOffset.UtcNow;
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