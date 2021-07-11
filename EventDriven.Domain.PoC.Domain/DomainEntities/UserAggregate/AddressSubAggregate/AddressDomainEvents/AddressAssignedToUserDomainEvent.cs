﻿using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents
{
    public class AddressAssignedToUserDomainEvent : DomainEventBase
    {
        public Guid AddressAssignerId;

        public Address NewAddress;
        public Guid UserId;
        public DateTimeOffset UtcNow;

        public AddressAssignedToUserDomainEvent(Guid userId, Address address, Guid addressAssignerId,
            DateTimeOffset utcNow)
        {
            UserId = userId;
            NewAddress = address;
            AddressAssignerId = addressAssignerId;
            UtcNow = utcNow;
        }

        public AddressAssignedToUserDomainEvent(
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