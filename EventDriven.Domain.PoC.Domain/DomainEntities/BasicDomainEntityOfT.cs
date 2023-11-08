using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.SharedKernel.BusinessRules;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.DomainErrors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using TrackableEntities.Common.Core;

namespace EventDriven.Domain.PoC.Domain.DomainEntities
{
    public abstract class BasicDomainEntity<TK> : ITrackable, ICreationAuditedEntity, IDeletionAuditedEntity,
        IModificationAuditedEntity
    {
        public static bool operator !=(BasicDomainEntity<TK> entity1,
            BasicDomainEntity<TK> entity2)
        {
            return !(entity1 == entity2);
        }

        public static bool operator ==(BasicDomainEntity<TK> entity1,
            BasicDomainEntity<TK> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.Id.ToString() == entity2.Id.ToString())
                return true;

            return false;
        }

        public override bool Equals(object entity)
        {
            return entity is BasicDomainEntity<TK> @base && this == @base;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <summary>
        ///     Checks if the current domain entity has an identity.
        /// </summary>
        /// <returns>
        ///     True if the domain entity is transient (i.e. has no identity yet),
        ///     false otherwise.
        /// </returns>
        public bool IsTransient()
        {
            return Id.Equals(default(TK));
        }

        #region Public Props

        public User ActivatedBy { get; private set; }
        public Guid? ActivatedById { get; set; }
        public bool Active { get; set; } = true;
        public DateTimeOffset? ActiveFrom { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ActiveTo { get; private set; }
        public User CreatedBy { get; private set; }
        public Guid? CreatedById { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }
        public DateTimeOffset? DateModified { get; set; }
        public User DeactivatedBy { get; private set; }
        public string DeactivateReason { get; private set; }
        public User DeletedBy { get; private set; }
        public Guid? DeletedById { get; set; }
        public string DeleteReason { get; private set; }
        public string Description { get; set; }
        public TK Id { get; set; }
        [NotMapped] public bool IsDraft { get; set; } = false;
        public User ModifiedBy { get; private set; }
        public Guid? ModifiedById { get; set; }
        public string Name { get; set; }
        public User ReactivatedBy { get; private set; }
        public string ReactivatedReason { get; private set; }
        public User UndeletedBy { get; private set; }

        public string UndeleteReason { get; private set; }

        internal void AssignCreatedBy(User creatorUser)
        {
            CreatedBy = creatorUser;
        }

        #endregion Public Props

        #region Public Methods

        public bool IsDeleted { get; set; }

        public void Activate(DateTimeOffset activeFrom, DateTimeOffset activeTo, User activatedBy)
        {
            Active = true;
            ActiveFrom = activeFrom;
            ActiveTo = activeTo;
            if (activatedBy != null)
                ActivatedBy = activatedBy;
        }

        public void ActivateWithNoActivator(DateTimeOffset activeFrom, DateTimeOffset activeTo)
        {
            Active = true;
            ActiveFrom = activeFrom;
            ActiveTo = activeTo;
            ActivatedById = null;
        }

        public void Deactivate(User deactivatedBy, string reason)
        {
            Active = false;
            DeactivatedBy = deactivatedBy;
            DeactivateReason = reason;
        }

        public void Delete(User deletedBy, string reason)
        {
            IsDeleted = true;
            DeletedBy = deletedBy;
            DeleteReason = reason;
        }

        public void Reactivate(DateTimeOffset activeFrom, DateTimeOffset activeTo, User reactivatedBy, string reason)
        {
            Active = true;
            ActiveFrom = activeFrom;
            ActiveTo = activeTo;
            ReactivatedBy = reactivatedBy;
            ReactivatedReason = reason;
        }

        public void Undelete(User undeletedBy, string reason)
        {
            IsDeleted = false;
            UndeletedBy = undeletedBy;
            UndeleteReason = reason;
        }

        #endregion Public Methods

        #region ITrackable

        [NotMapped] public ICollection<string> ModifiedProperties { get; set; }
        [NotMapped] public TrackingState TrackingState { get; set; }

        #endregion ITrackable

        #region Business rules

        private readonly List<BusinessRule> _brokenRules = new();

        public void ThrowExceptionIfInvalid()
        {
            _brokenRules.Clear();

            Validate();

            if (_brokenRules.Any())
            {
                var issues = new StringBuilder();

                foreach (var businessRule in _brokenRules)
                    issues.AppendLine(businessRule.Rule);

                throw new EntityIsInvalidException(issues.ToString());
            }
        }

        public abstract IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext
        );

        public IEnumerable<ValidationResult> Validate()
        {
            var validationErrors = new List<ValidationResult>();

            var ctx = new ValidationContext(this, null, null);

            Validator.TryValidateObject(this, ctx, validationErrors, true);

            return validationErrors;
        }

        protected void AddBrokenRule(BusinessRule businessRule)
        {
            _brokenRules.Add(businessRule);
        }

        #endregion Business rules

        #region Domain Events

        private List<IDomainEvent> _domainEvents;

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public bool IsSeed { get; set; } = false;

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken()) throw new BusinessRuleValidationException(rule);
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        #endregion Domain Events
    }
}