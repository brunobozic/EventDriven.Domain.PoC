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

namespace EventDriven.Domain.PoC.SharedKernel.DomainBaseAbstractions
{
    public abstract class DomainEntity<TK> : IHandlesConcurrency, ISoftDeletable, IDeactivatableEntity,
        ICreationAuditedEntity, IModificationAuditedEntity, IDeletionAuditedEntity, ITrackable
    {
        public TK Id { get; set; }

        [Required] [Column("Name", Order = 1)] public string Name { get; set; }

        [Column("Description", Order = 2)] public string Description { get; set; }

        #region IHandlesConcurrency

        public byte[] RowVersion { get; set; }

        #endregion IHandlesConcurrency

        #region ISoftDeletable

        [Required]
        [Column("Deleted", Order = 996)]
        public bool Deleted { get; set; } = false;

        #endregion ISoftDeletable

        public override bool Equals(object entity)
        {
            return entity is DomainEntity<TK> @base && this == @base;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(DomainEntity<TK> entity1,
            DomainEntity<TK> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.Id.ToString() == entity2.Id.ToString())
                return true;

            return false;
        }

        public static bool operator !=(DomainEntity<TK> entity1,
            DomainEntity<TK> entity2)
        {
            return !(entity1 == entity2);
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

        #region ICreationAuditedEntity

        [Required]
        [Column("DateCreated", Order = 1005)]
        public DateTimeOffset DateCreated { get; set; } = DateTimeOffset.UtcNow;

        public Guid? CreatedById { get; set; }
        public bool IsDraft { get; set; } = false;
        public bool IsSeed { get; set; } = false;

        #endregion ICreationAuditedEntity

        #region IModificationAuditedEntity

        [Column("DateModified", Order = 1010)] public DateTimeOffset? DateModified { get; set; }
        public Guid? ModifiedById { get; set; }

        #endregion IModificationAuditedEntity

        #region IDeletionAuditedEntity

        [Column("DateDeleted", Order = 1020)] public DateTimeOffset? DateDeleted { get; set; }
        public Guid? DeletedById { get; set; }

        #endregion IDeletionAuditedEntity

        #region IDeactivatableEntity

        [Required]
        [Column("IsActive", Order = 990)]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column("ActiveFrom", Order = 991)]
        public DateTimeOffset ActiveFrom { get; set; } = DateTimeOffset.UtcNow;

        [Required]
        [Column("ActiveTo", Order = 992)]
        public DateTimeOffset? ActiveTo { get; set; }

        #endregion IDeactivatableEntity

        #region ITrackable

        [NotMapped] public TrackingState TrackingState { get; set; }

        [NotMapped] public ICollection<string> ModifiedProperties { get; set; }

        #endregion ITrackable

        #region Business rules

        private readonly List<BusinessRule> _brokenRules = new();

        protected void AddBrokenRule(BusinessRule businessRule)
        {
            _brokenRules.Add(businessRule);
        }

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

        #endregion Business rules

        #region Domain Events

        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public Guid DeletedBy { get; set; }

        private List<IDomainEvent> _domainEvents;

        /// <summary>
        ///     Domain events occurred.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        ///     Add domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        /// <summary>
        ///     Clear domain events.
        /// </summary>
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken()) throw new BusinessRuleValidationException(rule);
        }

        #endregion Domain Events
    }
}