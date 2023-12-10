// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

using System;
using System.Collections.Generic;
using Elastic.CommonSchema;

namespace Framework.Serilog.Sink.Kafka.Http;

public interface IHttpAdapter
{
    Client Client { get; }
    IEnumerable<Exception> Exceptions { get; }
    Elastic.CommonSchema.Http Http { get; }
    Server Server { get; }
    Url Url { get; }
    User User { get; }
    UserAgent UserAgent { get; }
}