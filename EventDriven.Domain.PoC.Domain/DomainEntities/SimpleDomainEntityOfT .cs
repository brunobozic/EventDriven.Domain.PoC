using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.SharedKernel.BusinessRules;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
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
    public abstract class SimpleDomainEntityOfT<TK> : ITrackable
    {
        public override bool Equals(object entity)
        {
            return entity is SimpleDomainEntityOfT<TK> @base && this == @base;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(SimpleDomainEntityOfT<TK> entity1,
            SimpleDomainEntityOfT<TK> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.Id.ToString() == entity2.Id.ToString())
                return true;

            return false;
        }

        public static bool operator !=(SimpleDomainEntityOfT<TK> entity1,
            SimpleDomainEntityOfT<TK> entity2)
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

        #region Public Props

        [NotMapped] public bool IsDraft { get; } = false;

        public DateTimeOffset DateCreated { get; } = DateTimeOffset.UtcNow;
        public bool Deleted { get; } = false;

        public DateTimeOffset? DateDeleted { get; private set; }
        public DateTimeOffset? DateModified { get; private set; }
        public User LastModifiedBy { get; private set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public TK Id { get; set; }

        #endregion Public Props


        #region Public Methods

        #endregion Public Methods

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