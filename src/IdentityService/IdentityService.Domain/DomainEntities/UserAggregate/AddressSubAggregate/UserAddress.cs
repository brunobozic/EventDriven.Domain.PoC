using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharedKernel.DomainCoreInterfaces;

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

    public Guid UserId { get; }
    public long AddressId { get; }
    public Guid? UndeletedById { get; }
    public Guid? DeactivatedById { get; }
    public Guid? ReactivatedById { get; }

    #endregion FK

    #region ctor

    public static UserAddress NewDraft(User applicationUser, Address address, User creator)
    {
        var userAddress = new UserAddress
            { User = applicationUser, Address = address, UserRoleGuid = Guid.NewGuid() };
        userAddress.AssignCreatedBy(creator);

        // activated by the creator
        userAddress.Activate(DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEFAULT_ACTIVETO_VALUE_FOR_USERADDRESS),
            creator);

        return userAddress;
    }

    public static UserAddress NewActivatedDraft(User applicationUser, Address address, User creator)
    {
        var userAddress = new UserAddress
            { User = applicationUser, Address = address, UserRoleGuid = Guid.NewGuid() };
        userAddress.AssignCreatedBy(creator);

        // activated by the creator
        userAddress.Activate(DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEFAULT_ACTIVETO_VALUE_FOR_USERADDRESS),
            creator);
        userAddress.ActivatedById = creator.Id;

        return userAddress;
    }

    public static UserAddress NewInactiveDraft(User applicationUser, Address address, User creator)
    {
        var userAddress = new UserAddress
            { User = applicationUser, Address = address, UserRoleGuid = Guid.NewGuid() };

        // not activated, but still has a creator
        userAddress.AssignCreatedBy(creator);

        return userAddress;
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

    internal bool TheAddressHasBeenDeleted()
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods
}