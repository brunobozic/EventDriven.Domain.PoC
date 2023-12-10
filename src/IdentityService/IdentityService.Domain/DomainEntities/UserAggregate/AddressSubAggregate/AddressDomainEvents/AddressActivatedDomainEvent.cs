using SharedKernel.DomainImplementations.BaseClasses;

namespace IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate.AddressDomainEvents;

public class AddressActivatedDomainEvent : DomainEventBase
{
    public AddressActivatedDomainEvent(string line1, string line2, int? flatNr, string postalCode, int houseNumber,
        string houseNumberSuffix, string userComment, AddressType addressTypeId, string cityBlockId,
        string countryId,
        string townId, string countyId, string creatorUserEmail, string creatorUserUserName,
        string creatorUserFullName)
    {
    }
}