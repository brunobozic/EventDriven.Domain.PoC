using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace SharedKernel.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum GenderEnum
{
    [EnumMember(Value = "Male")] Male,

    [EnumMember(Value = "Female")] Female,

    [EnumMember(Value = "WontSay")] WontSay,

    [EnumMember(Value = "Unassigned")] Unassigned
}