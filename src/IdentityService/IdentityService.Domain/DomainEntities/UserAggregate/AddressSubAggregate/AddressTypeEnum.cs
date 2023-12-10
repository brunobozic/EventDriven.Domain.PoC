using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;

[JsonConverter(typeof(StringEnumConverter))]
public enum AddressTypeEnum : byte
{
    [Description("Stanovanje")] Stanovanja = 1,
    [Description("Prebivaliste")] Prebivalista = 2,
    [Description("Primary")] Primary = 3,
    [Description("Secondary")] Secondary = 4,
    [Description("Shipping")] Shipping = 5,
    [Description("Default")] Default = 6,
    [Description("Living")] Living = 7
}