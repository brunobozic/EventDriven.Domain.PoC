using SharedKernel.DomainCoreInterfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;

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

    #region Constructors

    private UserAddress() { }

    public static UserAddress NewDraft(User applicationUser, Address address, User creator)
    {
        var userAddress = new UserAddress
        {
            User = applicationUser,
            Address = address,
            UserRoleGuid = Guid.NewGuid()
        };

        userAddress.AssignCreatedBy(creator);
        userAddress.Activate(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEFAULT_ACTIVETO_VALUE_FOR_USERADDRESS), creator);

        return userAddress;
    }

    public static UserAddress NewActivatedDraft(User applicationUser, Address address, User creator)
    {
        var userAddress = new UserAddress
        {
            User = applicationUser,
            Address = address,
            UserRoleGuid = Guid.NewGuid(),
            ReactivatedById = creator.Id
        };

        userAddress.AssignCreatedBy(creator);
        userAddress.Activate(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEFAULT_ACTIVETO_VALUE_FOR_USERADDRESS), creator);

        return userAddress;
    }

    public static UserAddress NewInactiveDraft(User applicationUser, Address address, User creator)
    {
        var userAddress = new UserAddress
        {
            User = applicationUser,
            Address = address,
            UserRoleGuid = Guid.NewGuid()
        };

        userAddress.AssignCreatedBy(creator);

        return userAddress;
    }

    #endregion Constructors

    #region Public Methods

    public bool EnsureIsActive() => ActiveTo >= DateTimeOffset.UtcNow;
    public virtual bool IsDeactivated() => !Active;
    public virtual bool IsExpired(DateTimeOffset theDate) => ActiveTo < theDate;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => throw new NotImplementedException();

    internal bool TheAddressHasBeenDeleted() => Address.IsDeleted;

    #endregion Public Methods

    #region Helper Methods

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

    #endregion Helper Methods
}
