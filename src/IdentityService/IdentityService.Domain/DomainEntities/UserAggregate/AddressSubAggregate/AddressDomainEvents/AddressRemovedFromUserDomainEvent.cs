using System;
using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents;

public class AddressRemovedFromUserDomainEvent : DomainEventBase
{
    public Guid AddressRemoverId;

    public Address NewAddress;
    public Guid UserId;
    public DateTimeOffset UtcNow;

    public AddressRemovedFromUserDomainEvent(
        Guid asignee
        , string assigneeUsername
        , string assigneeEmail
        , long addressId
        , string addressLine1
        , bool addressActive
        , Guid addressGuid
        , int addressHouseNumber
        , string addressHouseNumberSuffix
        , int? addressFlatNr
        , long townId
        , string townName
        , DateTimeOffset townDateCreated
        , DateTimeOffset? townDateModified
        , string townZipCode
        , string addressPostalCode
        , DateTimeOffset addressDateCreated
        , DateTimeOffset? addressDateModified
        , long addressTypeId
        , string addressTypeName
        , bool addressTypeActive
        , string addressTypeDescription
        , Guid assignerId
        , string assignerUsername
        , string assignerEmail
        , DateTimeOffset utcNow)
    {
        Assignee = asignee;
        AssigneeUsername = assigneeUsername;
        AssigneeEmail = assigneeEmail;
        AddressId = addressId;
        AddressLine1 = addressLine1;
        AddressActive = addressActive;
        AddressIdGuid = addressGuid;
        AddressHouseNumber = addressHouseNumber;
        AddressHouseNumberSuffix = addressHouseNumberSuffix;
        AddressFlatNr = addressFlatNr;
        TownId = townId;
        TownName = townName;
        TownDateCreated = townDateCreated;
        TownDateModified = townDateModified;
        TownZipCode = townZipCode;
        AddressPostalCode = addressPostalCode;
        AddressDateCreated = addressDateCreated;
        AddressDateModified = addressDateModified;
        AddressTypeID = addressTypeId;
        AddressTypeName = addressTypeName;
        AddressTypeActive = addressTypeActive;
        AddressTypeDescription = addressTypeDescription;
        AssignerId = assignerId;
        AssignerUserName = assignerUsername;
        AssignerEmail = assignerEmail;
        UtcNow = utcNow;
    }

    public Guid Assignee { get; }
    public string AssigneeUsername { get; }
    public string AssigneeEmail { get; }
    public long AddressId { get; }
    public string AddressLine1 { get; }
    public bool AddressActive { get; }
    public Guid AddressIdGuid { get; }
    public int AddressHouseNumber { get; }
    public string AddressHouseNumberSuffix { get; }
    public int? AddressFlatNr { get; }
    public string AddressPostalCode { get; }
    public DateTimeOffset DateCreated { get; }
    public DateTimeOffset? DateModified { get; }
    public long TownId { get; }
    public string TownName { get; }
    public bool AddressTypeActive { get; }
    public string AddressTypeName { get; }
    public string AddressTypeDescription { get; }
    public string AssignerUserName { get; }
    public string AssignerEmail { get; }
    public DateTimeOffset TownDateCreated { get; }
    public DateTimeOffset? TownDateModified { get; }
    public string TownZipCode { get; }
    public DateTimeOffset AddressDateCreated { get; }
    public DateTimeOffset? AddressDateModified { get; }
    public long AddressTypeID { get; }
    public Guid AssignerId { get; }
}