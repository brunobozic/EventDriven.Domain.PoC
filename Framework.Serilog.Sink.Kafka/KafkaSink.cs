using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Framework.Kafka.Core.Contracts;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Json;
using Serilog.Sinks.PeriodicBatching;

namespace HT.Framework.Serilog.Sink.Kafka
{
    public class KafkaSink : PeriodicBatchingSink
    {
        private readonly ITextFormatter _formatter;
        private readonly IKafkaLoggingProducer _producer;

        public KafkaSink(
            IKafkaLoggingProducer producer,
            int batchSizeLimit,
            int period,
            ITextFormatter formatter = null
        ) : base(batchSizeLimit, TimeSpan.FromSeconds(period))
        {
            // TODO: Formatting.Json namespace
            _formatter = formatter ?? new JsonFormatter(renderMessage: true);
            _producer = producer;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var tasks = new List<Task>();

            foreach (var logEvent in events)
            {
                await using var render = new StringWriter(CultureInfo.InvariantCulture);
                _formatter.Format(logEvent, render);
                tasks.Add(_producer.WriteLogMessageAsync(render.ToString()));
            }

            await Task.WhenAll(tasks);
        }
    }
}