using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate.RoleDomainEvents;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate
{
    public class Role : BasicDomainEntity<long>, IAuditTrail
    {
        #region Public Props

        // public Guid RoleId { get; private set; }
        public Guid RoleIdGuid { get; set; }

        #endregion Public Props

        #region Navigation properties

        private readonly List<UserRole> _userRoles;

        public IReadOnlyCollection<UserRole> UserRoles => _userRoles;

        #endregion Navigation properties

        #region FK

        public Guid? ReactivatedById { get; set; }
        public Guid? DeactivatedById { get; set; }
        public Guid? UndeletedById { get; set; }

        #endregion FK

        #region ctor

        private Role()
        {
            _userRoles = new List<UserRole>();
        }

        public static Role NewDraft(
            string name
            , string description
            , User creatorUser
            , DateTimeOffset dateCreated
        )
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            var roleIdGuid = Guid.NewGuid();

            var role = new Role
            {
                Name = name.Trim(),
                Description = description.Trim(),
                RoleIdGuid = roleIdGuid
            };

            role.AddDomainEvent(new RoleCreatedDomainEvent(
                name
                , description
                , roleIdGuid
                , creatorUser.Id
                , creatorUser.UserName
                , creatorUser.Email
                , dateCreated
            ));

            return role;
        }

        public static Role NewActiveDraft(
            string name
            , string description
            , DateTimeOffset from
            , DateTimeOffset to
            , User creatorUser
        )
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            var roleIdGuid = Guid.NewGuid();

            var role = new Role
            {
                Name = name.Trim(),
                Description = description.Trim(),
                RoleIdGuid = roleIdGuid
            };

            if (creatorUser != null) // when seeding, the creator user may not yet be inserted - hence this might be null
            {
                role.Activate(from, to, creatorUser);
                role.AssignCreatedBy(creatorUser);

                role.AddDomainEvent(new RoleCreatedDomainEvent(
                    name
                    , description
                    , roleIdGuid
                    , creatorUser.Id
                    , creatorUser.UserName
                    , creatorUser.Email
                    , DateTimeOffset.UtcNow
                ));
            }
            else
            {
                role.AddDomainEvent(new RoleCreatedDomainEvent(
                    name
                    , description
                    , roleIdGuid
                    , null
                    , "Seed"
                    , "Seed"
                    , DateTimeOffset.UtcNow
                ));
            }

            return role;
        }

        #endregion ctor

        #region Public methods

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public void AssignCreatedBy(User creatorUser)
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        {
            base.AssignCreatedBy(creatorUser);
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        /// <summary>
        ///     Returns the activity validity of the entity
        /// </summary>
        /// <returns></returns>
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

        #endregion Public methods
    }
}