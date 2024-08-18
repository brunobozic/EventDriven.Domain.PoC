using IdentityService.Domain.DomainEntities.DomainExceptions;
using SharedKernel.DomainCoreInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;

public class AddressType : BasicDomainEntity<long>, IAuditTrail
{
    public bool TheAddressTypeHasBeenDeleted() => IsDeleted;

    #region Public properties

    public Guid? ReactivatedById { get; private set; }
    public Guid? DeactivatedById { get; private set; }
    public Guid? UndeletedById { get; private set; }

    #endregion Public properties

    #region Constructors

    private AddressType() { }

    public static AddressType NewDraft(string name, string description, User creatorUser)
    {
        ValidateParameters(name, description);

        var addressType = new AddressType
        {
            Name = name.Trim(),
            Description = description.Trim()
        };

        addressType.AssignCreatedBy(creatorUser);

        return addressType;
    }

    public static AddressType NewActiveDraft(string name, string description, User creatorUser, DateTimeOffset activeFrom, DateTimeOffset activeTo)
    {
        ValidateParameters(name, description);

        var addressType = new AddressType
        {
            Name = name.Trim(),
            Description = description.Trim()
        };

        addressType.Activate(activeFrom, activeTo, creatorUser);
        addressType.AssignCreatedBy(creatorUser);

        return addressType;
    }

    #endregion Constructors

    #region Public methods

    public void ChangeDescription(string newDescription)
    {
        ExecuteWithLogging(nameof(ChangeDescription), () =>
        {
            EnsureIsActive();
            Description = newDescription;
            return true;
        });
    }

    public bool EnsureIsActive() => ActiveTo >= DateTimeOffset.UtcNow;
    public virtual bool IsDeactivated() => !Active;
    public virtual bool IsExpired(DateTimeOffset theDate) => ActiveTo < theDate;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => throw new NotImplementedException();

    #endregion Public methods

    #region Helper methods

    private static void ValidateParameters(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentNullException(nameof(description));
    }

    private static T ExecuteWithLogging<T>(string methodName, Func<T> action)
    {
        using (Serilog.Context.LogContext.PushProperty("MethodName", methodName))
        {
            try
            {
                Log.Information("{MethodName} called", methodName);
                return action();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName}", methodName);
                throw;
            }
        }
    }

    #endregion Helper methods
}
