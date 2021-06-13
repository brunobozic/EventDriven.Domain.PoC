using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EventDriven.Domain.PoC.Domain.DomainEntities.DomainExceptions;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.UserDomainEvents.CUD;
using EventDriven.Domain.PoC.SharedKernel.DomainCoreInterfaces;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate
{
    public class Address : BasicDomainEntity<long>, IAuditTrail
    {
        #region Public properties

        public string Line1 { get; private set; }
        public string Line2 { get; private set; }
        public string PostalCode { get; private set; }
        public int HouseNumber { get; private set; }
        public string HouseNumberSuffix { get; private set; }
        public int? FlatNr { get; private set; }
        public string UserComment { get; private set; }
        public Guid AddressIdGuid { get; private set; }

        #endregion Public properties

        #region Navigation properties

        private AddressType AddressType { get; set; }

        public string GetAddressTypeName()
        {
            if (AddressType != null && AddressType.EnsureIsActive() && AddressType.Active && !AddressType.Deleted)
                return AddressType.Name;
            return "Not available (inactive, deleted, does not exist)";
        }

        public Town Town { get; private set; }
        public County County { get; private set; }
        public CityBlock CityBlock { get; private set; }
        public Country Country { get; private set; }

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly List<UserAddress> _userAddresses;

        public IReadOnlyCollection<UserAddress> UserAddresses => _userAddresses;
        public long? ReactivatedById { get; private set; }
        public long? DeactivatedById { get; private set; }

        // ReSharper disable once IdentifierTypo
        public long? UndeletedById { get; private set; }
        public long AddressTypeId { get; private set; }

        #endregion Navigation properties

        #region ctor

        private Address()
        {
            _userAddresses = new List<UserAddress>();
        }

        public static Address NewDraft(
            string name
            , string description
            , string line1
            , string line2
            , int? flatNr
            , string postalCode
            , int houseNumber
            , string houseNumberSuffix
            , string userComment
            , AddressType addressType
            , string cityBlockName
            , string countryName
            , string countyName
            , string townName
            , User creatorUser
            , DateTimeOffset dateCreated
        )
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            var addressIdGuid = Guid.NewGuid();

            var address = new Address
            {
                Name = name.Trim(),
                Description = description.Trim(),
                AddressIdGuid = addressIdGuid,
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
                addressIdGuid
                , line1
                , line2
                , flatNr
                , postalCode
                , houseNumber
                , houseNumberSuffix
                , userComment
                , addressType
                , cityBlockName
                , countryName
                , townName
                , countyName
                , creatorUser.Email
                , creatorUser.UserName
                , creatorUser.FullName
            ));

            return address;
        }


        public static Address NewActiveDraft(
            string name
            , string description
            , string line1
            , string line2
            , int? flatNr
            , string postalCode
            , int houseNumber
            , string houseNumberSuffix
            , string userComment
            , AddressType addressType
            , string cityBlockName
            , string countryName
            , string townName
            , string countyName
            , User creatorUser
            , DateTimeOffset dateCreated
            , DateTimeOffset activeFrom
            , DateTimeOffset activeTo
        )
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            var addressIdGuid = Guid.NewGuid();

            var address = new Address
            {
                Name = name.Trim(),
                Description = description.Trim(),
                AddressIdGuid = addressIdGuid,
                Line1 = line1,
                Line2 = line2,
                FlatNr = flatNr,
                PostalCode = postalCode,
                HouseNumber = houseNumber,
                HouseNumberSuffix = houseNumberSuffix,
                UserComment = userComment
            };

            if (activeFrom == DateTimeOffset.MinValue) activeFrom = DateTimeOffset.UtcNow;

            if (activeTo == DateTimeOffset.MinValue)
                activeTo = DateTimeOffset.UtcNow.AddYears(Consts.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES);

            address.Activate(activeFrom, activeTo, creatorUser);

            address.AssignCreatedBy(creatorUser);

            address.AssignAddressType(addressType, creatorUser);

            address.AddDomainEvent(new AddressCreatedDomainEvent(
                addressIdGuid
                , line1
                , line2
                , flatNr
                , postalCode
                , houseNumber
                , houseNumberSuffix
                , userComment
                , addressType
                , cityBlockName
                , countryName
                , townName
                , countyName
                , creatorUser.Email
                , creatorUser.UserName
                , creatorUser.FullName
            ));

            return address;
        }

        #endregion ctor

        #region Public methods

        public void SetLine1(string line1, User changedBy)
        {
            if (EnsureIsActive() && !IsDeleted() && !IsDeactivated())
            {
                Line1 = line1;
                AddDomainEvent(new UserUpdatedAddressDomainEvent
                    {AddressId = Id, UserId = new UserId(changedBy.UserIdGuid)});
            }
            else
            {
                throw new DomainException(
                    "The address is either deactivated, deleted or has expired, unable to change the line 1 property.");
            }
        }

        public void SetLine2(string line2, User changedBy)
        {
            if (EnsureIsActive() && !IsDeleted() && !IsDeactivated())
            {
                Line2 = line2;
                AddDomainEvent(new UserUpdatedAddressDomainEvent
                    {AddressId = Id, UserId = new UserId(changedBy.UserIdGuid)});
            }
            else
            {
                throw new DomainException(
                    "The address is either deactivated, deleted or has expired, unable to change the line 2 property.");
            }
        }

        public void SetFlatNr(int flatNumber, User changedBy)
        {
            if (EnsureIsActive() && !IsDeleted() && !IsDeactivated())
            {
                FlatNr = flatNumber;
                AddDomainEvent(new UserUpdatedAddressDomainEvent
                    {AddressId = Id, UserId = new UserId(changedBy.UserIdGuid)});
            }
            else
            {
                throw new DomainException(
                    "The address is either deactivated, deleted or has expired, unable to change the flat number.");
            }
        }

        public void SetPostalCode(string postalCode, User changedBy)
        {
            if (EnsureIsActive() && !IsDeleted() && !IsDeactivated())
            {
                PostalCode = postalCode;
                AddDomainEvent(new UserUpdatedAddressDomainEvent
                    {AddressId = Id, UserId = new UserId(changedBy.UserIdGuid)});
            }
            else
            {
                throw new DomainException(
                    "The address is either deactivated, deleted or has expired, unable to change the postal code.");
            }
        }

        public void SetHouseNumber(int houseNumber, User changedBy)
        {
            if (EnsureIsActive() && !IsDeleted() && !IsDeactivated())
            {
                HouseNumber = houseNumber;
                AddDomainEvent(new UserUpdatedAddressDomainEvent
                    {AddressId = Id, UserId = new UserId(changedBy.UserIdGuid)});
            }
            else
            {
                throw new DomainException(
                    "The address is either deactivated, deleted or has expired, unable to change the house number.");
            }
        }

        public void SetHouseNumberSuffix(string houseNumberSuffix, User changedBy)
        {
            if (EnsureIsActive() && !IsDeleted() && !IsDeactivated())
            {
                HouseNumberSuffix = houseNumberSuffix;
                AddDomainEvent(new UserUpdatedAddressDomainEvent
                    {AddressId = Id, UserId = new UserId(changedBy.UserIdGuid)});
            }
            else
            {
                throw new DomainException(
                    "The address is either deactivated, deleted or has expired, unable to change the house number suffix.");
            }
        }

        public void AssignAddressType(AddressType addressType, User changedBy)
        {
            if (EnsureIsActive() && !IsDeleted() && !IsDeactivated())
            {
                if (addressType.EnsureIsActive() && !addressType.IsDeleted() && !IsDeactivated())
                {
                    AddressType = addressType;
                    AddDomainEvent(new UserUpdatedAddressDomainEvent
                        {AddressId = Id, UserId = new UserId(changedBy.UserIdGuid)});
                }
                else
                {
                    throw new DomainException(
                        "The address type is either deactivated, deleted or has expired, unable to assign it.");
                }
            }
            else
            {
                throw new DomainException(
                    "The address is either deactivated, deleted or has expired, unable to assign an address type to it.");
            }
        }

        private bool EnsureIsActive()
        {
            return ActiveTo >= DateTimeOffset.UtcNow;
        }

        public virtual bool IsDeleted()
        {
            return Deleted;
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

        #endregion Public methods
    }
}