using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.SharedKernel.DomainImplementations.BaseClasses;
using System;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Addresses
{
    public class AssignAddressToUserCommand : CommandBase<AddressAssignmentDto>
    {
        public AssignAddressToUserCommand(
            Guid userId
            , string addressTypeName
            , string addressName
            , string description
            , string line1
            , string line2
            , int? flatNr
            , string postalCode
            , int houseNumber
            , string houseNumberSuffix
            , string userComment
            , string cityBlockName
            , string countryName
            , string townName
            , string countyName
            , DateTimeOffset activeFrom
            , DateTimeOffset activeTo
        )
        {
            UserId = userId;
            AddressTypeName = addressTypeName;
            AddressName = addressName;
            Description = description;
            Line1 = line1;
            Line2 = line2;
            FlatNr = flatNr;
            PostalCode = postalCode;
            HouseNumber = houseNumber;
            HouseNumberSuffix = houseNumberSuffix;
            UserComment = userComment;
            CityBlockName = cityBlockName;
            CountryName = countryName;
            TownName = townName;
            CountyName = countyName;
            ActiveFrom = activeFrom;
            ActiveTo = activeTo;
        }

        public Guid UserId { get; set; }
        public User AssignerUser { get; set; }
        public string AddressTypeName { get; set; }
        public string AddressName { get; set; }
        public string Description { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public int? FlatNr { get; set; }
        public string PostalCode { get; set; }
        public int HouseNumber { get; set; }
        public string HouseNumberSuffix { get; set; }
        public string UserComment { get; set; }
        public string CityBlockName { get; set; }
        public string CountryName { get; set; }
        public string TownName { get; set; }
        public string CountyName { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset ActiveTo { get; set; }
        public string Origin { get; set; }
    }
}