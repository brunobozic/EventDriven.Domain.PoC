﻿using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PersonTypeEnum : byte
    {
        [Description("Fizička")] Fizicka = 1,
        [Description("Pravna")] Pravna = 2,
        [Description("Default")] Default = 3
    }
}