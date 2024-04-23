using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SharedKernel.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum RoleEnum
{
    [EnumMember(Value = "Admin")] Admin,

    [EnumMember(Value = "User")] User,

    [EnumMember(Value = "Unassigned")] Unassigned
}