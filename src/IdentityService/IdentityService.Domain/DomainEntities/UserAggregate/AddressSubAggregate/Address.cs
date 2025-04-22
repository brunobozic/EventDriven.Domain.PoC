using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents;
using IdentityService.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using Serilog;
using SharedKernel.DomainContracts;
using SharedKernel.DomainCoreInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;

public sealed class Address : BasicDomainEntity<long>, IAuditTrail, IAggregateRoot
{
    #region Public Properties

    public string Line1 { get; private set; }
    public string Line2 { get; private set; }
    public string PostalCode { get; private set; }
    public int HouseNumber { get; private set; }
    public string HouseNumberSuffix { get; private set; }
    public int? FlatNr { get; private set; }
    public string UserComment { get; private set; }
    public Guid AddressIdGuid { get; private set; }

    #endregion Public Properties

    #region Navigation Properties

    private readonly List<UserAddress> _userAddresses = new();
    public IReadOnlyCollection<UserAddress> UserAddresses => _userAddresses.AsReadOnly();

    private AddressType AddressType { get; set; }

    public string GetAddressTypeName() =>
        AddressType != null && AddressType.EnsureIsActive() && AddressType.Active && !AddressType.IsDeleted
            ? AddressType.Name
            : "Not available (inactive, deleted, does not exist)";

    public Town Town { get; }
    public County County { get; }
    public CityBlock CityBlock { get; }
    public Country Country { get; }

    public Guid? ReactivatedById { get; private set; }
    public Guid? DeactivatedById { get; private set; }
    public Guid? UndeletedById { get; private set; }

    public long AddressTypeId { get; }

    #endregion Navigation Properties

    #region Constructors

    private Address() { }

    public static Address NewDraft(
        string name, string description, string line1, string line2, int? flatNr, string postalCode,
        int houseNumber, string houseNumberSuffix, string userComment, AddressType addressType, string cityBlockName,
        string countryName, string countyName, string townName, User creatorUser, DateTimeOffset dateCreated)
    {
        ValidateParameters(name, description);

        var address = new Address
        {
            Name = name.Trim(),
            Description = description.Trim(),
            AddressIdGuid = Guid.NewGuid(),
            Line1 = line1,
            Line2 = line2,
            FlatNr = flatNr,
            PostalCode = postalCode,
            HouseNumber = houseNumber,
            HouseNumberSuffix = houseNumberSuffix,
            UserComment = userComment
        };

        address.AssignAddressType(addressType, creatorUser);
        address.AddDomainEvent(new AddressCreatedDomainEvent(
            address.AddressIdGuid, line1, line2, flatNr, postalCode, houseNumber, houseNumberSuffix, userComment,
            addressType, cityBlockName, countryName, townName, countyName, creatorUser.Email, creatorUser.UserName,
            creatorUser.FullName));

        return address;
    }

    public static Address NewActiveDraft(
        string name, string description, string line1, string line2, int? flatNr, string postalCode, int houseNumber,
        string houseNumberSuffix, string userComment, AddressType addressType, string cityBlockName, string countryName,
        string townName, string countyName, User creatorUser, DateTimeOffset dateCreated, DateTimeOffset activeFrom,
        DateTimeOffset activeTo)
    {
        ValidateParameters(name, description);

        var address = new Address
        {
            Name = name.Trim(),
            Description = description.Trim(),
            AddressIdGuid = Guid.NewGuid(),
            Line1 = line1,
            Line2 = line2,
            FlatNr = flatNr,
            PostalCode = postalCode,
            HouseNumber = houseNumber,
            HouseNumberSuffix = houseNumberSuffix,
            UserComment = userComment
        };

        address.Activate(activeFrom == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow : activeFrom,
            activeTo == DateTimeOffset.MinValue
                ? DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES)
                : activeTo, creatorUser);
        address.AssignCreatedBy(creatorUser);
        address.AssignAddressType(addressType, creatorUser);
        address.AddDomainEvent(new AddressCreatedDomainEvent(
            address.AddressIdGuid, line1, line2, flatNr, postalCode, houseNumber, houseNumberSuffix, userComment,
            addressType, cityBlockName, countryName, townName, countyName, creatorUser.Email, creatorUser.UserName,
            creatorUser.FullName));

        return address;
    }

    #endregion Constructors

    #region Public Methods

    public void SetLine1(string line1, User changedBy)
    {
        ExecuteWithLogging(nameof(SetLine1), () =>
        {
            EnsureIsActive();
            Line1 = line1;
            AddDomainEvent(new UserUpdatedAddressDomainEvent { AddressId = Id, UserId = changedBy.Id });
            return true;
        }, this);
    }

    public void SetLine2(string line2, User changedBy)
    {
        ExecuteWithLogging(nameof(SetLine2), () =>
        {
            EnsureIsActive();
            Line2 = line2;
            AddDomainEvent(new UserUpdatedAddressDomainEvent { AddressId = Id, UserId = changedBy.Id });
            return true;
        }, this);
    }

    public void SetFlatNr(int flatNumber, User changedBy)
    {
        ExecuteWithLogging(nameof(SetFlatNr), () =>
        {
            EnsureIsActive();
            FlatNr = flatNumber;
            AddDomainEvent(new UserUpdatedAddressDomainEvent { AddressId = Id, UserId = changedBy.Id });
            return true;
        }, this);
    }

    public void SetPostalCode(string postalCode, User changedBy)
    {
        ExecuteWithLogging(nameof(SetPostalCode), () =>
        {
            EnsureIsActive();
            PostalCode = postalCode;
            AddDomainEvent(new UserUpdatedAddressDomainEvent { AddressId = Id, UserId = changedBy.Id });
            return true;
        }, this);
    }

    public void SetHouseNumber(int houseNumber, User changedBy)
    {
        ExecuteWithLogging(nameof(SetHouseNumber), () =>
        {
            EnsureIsActive();
            HouseNumber = houseNumber;
            AddDomainEvent(new UserUpdatedAddressDomainEvent { AddressId = Id, UserId = changedBy.Id });
            return true;
        }, this);
    }

    public void SetHouseNumberSuffix(string houseNumberSuffix, User changedBy)
    {
        ExecuteWithLogging(nameof(SetHouseNumberSuffix), () =>
        {
            EnsureIsActive();
            HouseNumberSuffix = houseNumberSuffix;
            AddDomainEvent(new UserUpdatedAddressDomainEvent { AddressId = Id, UserId = changedBy.Id });
            return true;
        }, this);
    }

    public void AssignAddressType(AddressType addressType, User changedBy)
    {
        ExecuteWithLogging(nameof(AssignAddressType), () =>
        {
            EnsureIsActive();
            AddressType = addressType;
            AddDomainEvent(new UserUpdatedAddressDomainEvent { AddressId = Id, UserId = changedBy.Id });
            return true;
        }, this);
    }

    public bool EnsureIsActive() => ActiveTo >= DateTimeOffset.UtcNow;
    public bool IsDeactivated() => !Active;
    public bool IsExpired(DateTimeOffset theDate) => ActiveTo < theDate;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => throw new NotImplementedException();

    internal bool TheAddressHasBeenDeleted() => IsDeleted;

    #endregion Public Methods

    #region Helper Methods

    private static void ValidateParameters(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentNullException(nameof(description));
    }

    private static T ExecuteWithLogging<T>(string methodName, Func<T> action, Address address)
    {
        using (SerilogHelper.PushMethodSpecificProperties(address, methodName))
        {
            try
            {
                Log.Information("{MethodName} called for address {AddressId}", methodName, address.Id);
                return action();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in {MethodName} for address {AddressId}", methodName, address.Id);
                throw;
            }
        }
    }

    #endregion Helper Methods
}
