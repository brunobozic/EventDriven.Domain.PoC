using Framework.Kafka.Core.Contracts;
using Serilog;
using Serilog.Configuration;
using Serilog.Formatting;

namespace HT.Framework.Serilog.Sink.Kafka
{
    public static class LoggerConfigurationKafkaExtensions
    {
        /// <summary>
        ///     Adds a sink that writes log events to a Kafka topic in the broker endpoints.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="customTextFormatter"></param>
        /// <param name="batchSizeLimit">The maximum number of events to include in a single batch.</param>
        /// <param name="period">The time in seconds to wait between checking for event batches.</param>
        /// <param name="producer"></param>
        /// <returns></returns>
        public static LoggerConfiguration Kafka(
            this LoggerSinkConfiguration loggerConfiguration,
            IKafkaLoggingProducer producer,
            ITextFormatter customTextFormatter,
            int batchSizeLimit = 50,
            int period = 5
        )
        {
            var sink = new KafkaSink(
                producer,
                batchSizeLimit,
                period,
                customTextFormatter
            );

            return loggerConfiguration.Sink(sink);
        }
    }
}