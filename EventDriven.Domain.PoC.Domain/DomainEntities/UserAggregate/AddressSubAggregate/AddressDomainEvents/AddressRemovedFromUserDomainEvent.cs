using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents
{
    public class AddressRemovedFromUserDomainEvent : DomainEventBase
    {
        public long AddressRemoverId;

        public Address NewAddress;
        public long UserId;
        public DateTimeOffset UtcNow;

        public AddressRemovedFromUserDomainEvent(long userId, Address address, long addressRemoverId,
            DateTimeOffset utcNow)
        {
            UserId = userId;
            NewAddress = address;
            AddressRemoverId = addressRemoverId;
            UtcNow = utcNow;
        }

        public AddressRemovedFromUserDomainEvent(
            string line1
            , string line2
            , int? flatNr
            , string postalCode
            , int houseNumber
            , string houseNumberSuffix
            , string userComment
            , string addressTypeName
            , string cityBlockId
            , string countryId
            , string townId
            , string countyId
            , string creatorUserEmail
            , string creatorUserUserName
            , string creatorUserFullName
        )
        {
        }
    }
}