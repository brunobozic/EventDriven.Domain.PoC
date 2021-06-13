using System;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents
{
    public class AddressCreatedDomainEvent : DomainEventBase
    {
        public AddressId AddressId;

        public AddressCreatedDomainEvent(Guid addressId, string line1, string line2, int? flatNr, string postalCode,
            int houseNumber,
            string houseNumberSuffix, string userComment, AddressType addressTypeId, string cityBlockId,
            string countryId,
            string townId, string countyId, string creatorUserEmail, string creatorUserUserName,
            string creatorUserFullName)
        {
            AddressId = new AddressId(addressId);
        }
    }
}