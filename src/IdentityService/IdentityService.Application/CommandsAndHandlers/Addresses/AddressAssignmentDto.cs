using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;

namespace IdentityService.Application.CommandsAndHandlers.Addresses;

public class AddressAssignmentDto
{
    public string RoleName { get; set; }
    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public CityBlock CityBlock { get; set; }
    public Country Country { get; set; }
    public County County { get; set; }
    public int? FlatNr { get; set; }
    public int HouseNumber { get; set; }
    public string HouseNumberSuffix { get; set; }
    public Town Town { get; set; }
    public string PostalCode { get; set; }
    public string UserComment { get; set; }
    public string AsigneeUserName { get; set; }
}