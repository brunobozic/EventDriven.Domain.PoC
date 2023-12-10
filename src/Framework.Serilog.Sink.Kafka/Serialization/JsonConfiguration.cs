// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Elastic.CommonSchema.Serialization;

namespace Framework.Serilog.Sink.Kafka.Serialization;

internal static class JsonConfiguration
{
    internal static readonly JsonSerializerOptions SerializerOptions = new()
    {
        IgnoreNullValues = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = new SnakeCaseJsonNamingPolicy(),
        Converters =
        {
            new DictionaryJsonConverterFactory(),
            new BaseJsonConverterFactory()
        }
    };

    internal static readonly JsonConverter<DateTimeOffset> DateTimeOffsetConverter =
        (JsonConverter<DateTimeOffset>)SerializerOptions.GetConverter(typeof(DateTimeOffset));

    internal static readonly MetaDataDictionaryConverter MetaDataDictionaryConverter = new();

    internal static readonly BaseJsonConverter BaseConverter = new();
}