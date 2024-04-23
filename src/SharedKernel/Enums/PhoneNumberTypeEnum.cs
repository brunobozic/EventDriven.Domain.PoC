using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace SharedKernel.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum PhoneNumberTypeEnum
{
    [EnumMember(Value = "Main")] Main,

    [EnumMember(Value = "Home")] Home,

    [EnumMember(Value = "Work")] Work,

    [EnumMember(Value = "Unassigned")] Unassigned
}