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

namespace EventDriven.Domain.PoC.SharedKernel.DomainBaseAbstractions
{
    public abstract class JournalEntityOfT<TK> : ITrackable
    {
        #region Public methods

        public override bool Equals(object entity)
        {
            return entity is JournalEntityOfT<TK> @base && this == @base;
        }

        public override int GetHashCode()
        {
            return JournalId.GetHashCode();
        }

        public static bool operator ==(JournalEntityOfT<TK> entity1,
            JournalEntityOfT<TK> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;

            if ((object)entity1 == null || (object)entity2 == null)
                return false;

            if (entity1.JournalId.ToString() == entity2.JournalId.ToString())
                return true;

            return false;
        }

        public static bool operator !=(JournalEntityOfT<TK> entity1,
            JournalEntityOfT<TK> entity2)
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
            return JournalId.Equals(default(TK));
        }

        #endregion Public methods

        #region Public Props

        public DateTimeOffset DateCreated { get; } = DateTimeOffset.UtcNow;
        public bool Deleted { get; } = false;

        public DateTimeOffset? DateDeleted { get; private set; }

        public TK JournalId { get; private set; }

        #endregion Public Props

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