using IdentityService.Domain.DomainEntities.UserAggregate;
using SharedKernel.BusinessRules;
using SharedKernel.DomainContracts;
using SharedKernel.DomainCoreInterfaces;
using SharedKernel.DomainImplementations.DomainErrors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TrackableEntities.Common.Core;

namespace IdentityService.Domain.DomainEntities;

public abstract class BasicDomainEntity<TK> : ITrackable, ICreationAuditedEntity, IDeletionAuditedEntity, IModificationAuditedEntity
{
    public static bool operator !=(BasicDomainEntity<TK> entity1, BasicDomainEntity<TK> entity2) => !(entity1 == entity2);

    public static bool operator ==(BasicDomainEntity<TK> entity1, BasicDomainEntity<TK> entity2) =>
        entity1 switch
        {
            null when entity2 is null => true,
            null or { } when entity2 is null => false,
            _ => entity1.Id.Equals(entity2.Id)
        };

    public override bool Equals(object entity) => entity is BasicDomainEntity<TK> other && this == other;

    public override int GetHashCode() => Id.GetHashCode();

    public bool IsTransient() => Id.Equals(default(TK));

    #region Public Props

    public User? ActivatedBy { get; private set; }
    public Guid? ActivatedById { get; set; }
    public bool Active { get; set; } = true;
    public DateTimeOffset? ActiveFrom { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ActiveTo { get; set; }
    public User? CreatedBy { get; private set; }
    public Guid? CreatedById { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateDeleted { get; set; }
    public DateTimeOffset? DateModified { get; set; }
    public User? DeactivatedBy { get; private set; }
    public string DeactivateReason { get; private set; } = string.Empty;
    public User? DeletedBy { get; private set; }
    public Guid? DeletedById { get; set; }
    public string DeleteReason { get; private set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TK Id { get; set; }
    [NotMapped] public bool IsDraft { get; set; } = false;
    public User? ModifiedBy { get; }
    public Guid? ModifiedById { get; set; }
    public string Name { get; set; } = string.Empty;
    public User? ReactivatedBy { get; private set; }
    public string ReactivatedReason { get; private set; } = string.Empty;
    public User? UndeletedBy { get; private set; }
    public string UndeleteReason { get; private set; } = string.Empty;

    internal void AssignCreatedBy(User creatorUser) => CreatedBy = creatorUser;

    #endregion Public Props

    #region Public Methods

    public bool IsDeleted { get; set; }

    public void Activate(DateTimeOffset activeFrom, DateTimeOffset activeTo, User activatedBy)
    {
        (Active, ActiveFrom, ActiveTo) = (true, activeFrom, activeTo);
        if (activatedBy != null)
            ActivatedBy = activatedBy;
    }

    public void ActivateWithNoActivator(DateTimeOffset activeFrom, DateTimeOffset activeTo)
    {
        (Active, ActiveFrom, ActiveTo, ActivatedById) = (true, activeFrom, activeTo, null);
    }

    public void Deactivate(User deactivatedBy, string reason)
    {
        (Active, DeactivatedBy, DeactivateReason) = (false, deactivatedBy, reason);
    }

    public void Delete(User deletedBy, string reason)
    {
        (IsDeleted, DeletedBy, DeleteReason) = (true, deletedBy, reason);
    }

    public void Reactivate(DateTimeOffset activeFrom, DateTimeOffset activeTo, User reactivatedBy, string reason)
    {
        (Active, ActiveFrom, ActiveTo, ReactivatedBy, ReactivatedReason) = (true, activeFrom, activeTo, reactivatedBy, reason);
    }

    public void Undelete(User undeletedBy, string reason)
    {
        (IsDeleted, UndeletedBy, UndeleteReason) = (false, undeletedBy, reason);
    }

    #endregion Public Methods

    #region ITrackable

    [NotMapped] public ICollection<string> ModifiedProperties { get; set; } = new List<string>();
    [NotMapped] public TrackingState TrackingState { get; set; }

    #endregion ITrackable

    #region Business rules

    private readonly List<BusinessRule> _brokenRules = new();

    public void ThrowExceptionIfInvalid()
    {
        _brokenRules.Clear();
        Validate();
        if (_brokenRules.Any())
            throw new EntityIsInvalidException(string.Join(Environment.NewLine, _brokenRules.Select(br => br.Rule)));
    }

    public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);

    public IEnumerable<ValidationResult> Validate()
    {
        var validationErrors = new List<ValidationResult>();
        var ctx = new ValidationContext(this);
        Validator.TryValidateObject(this, ctx, validationErrors, true);
        return validationErrors;
    }

    protected void AddBrokenRule(BusinessRule businessRule) => _brokenRules.Add(businessRule);

    #endregion Business rules

    #region Domain Events

    private List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public bool IsSeed { get; set; } = false;

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken()) throw new BusinessRuleValidationException(rule);
    }

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    #endregion Domain Events
}
