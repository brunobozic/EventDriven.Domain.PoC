using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using IdentityService.Domain.DomainEntities.UserAggregate;
using SharedKernel.BusinessRules;
using SharedKernel.DomainContracts;
using SharedKernel.DomainImplementations.DomainErrors;
using TrackableEntities.Common.Core;

namespace IdentityService.Domain.DomainEntities;

public abstract class SimpleDomainEntityOfT<TK> : ITrackable
{
    public static bool operator !=(SimpleDomainEntityOfT<TK> entity1,
        SimpleDomainEntityOfT<TK> entity2)
    {
        return !(entity1 == entity2);
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

    public override bool Equals(object entity)
    {
        return entity is SimpleDomainEntityOfT<TK> @base && this == @base;
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

    public DateTimeOffset DateCreated { get; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DateDeleted { get; }
    public DateTimeOffset? DateModified { get; }
    public bool Deleted { get; } = false;
    public string Description { get; set; }
    public TK Id { get; set; }
    [NotMapped] public bool IsDraft { get; } = false;
    public User LastModifiedBy { get; }

    public string Name { get; set; }

    #endregion Public Props

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

    /// <summary>
    ///     Domain events occurred.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

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

    /// <summary>
    ///     Add domain event.
    /// </summary>
    /// <param name="domainEvent"></param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    #endregion Domain Events
}