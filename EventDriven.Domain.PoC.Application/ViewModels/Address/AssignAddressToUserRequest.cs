using System;

namespace EventDriven.Domain.PoC.Application.ViewModels.Address
{
    public class AssignAddressToUserRequest
    {
        public Guid UserId { get; set; }
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
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset ActiveTo { get; set; }
    }
}