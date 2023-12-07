using Serilog.Configuration;
using Serilog;
using System;
using EventDriven.Domain.PoC.Api.Rest.Enrichers;

namespace EventDriven.Domain.PoC.Api.Rest.Extensions
{
    public static class LoggingExtensions
    {
        public static LoggerConfiguration WithKafkaConsumerOffset(
            this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<KafkaConsumerOffsetEnricher>();
        }

        public static LoggerConfiguration WithKafkaConsumerTopic(
          this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<KafkaConsumerTopicEnricher>();
        }

        public static LoggerConfiguration WithKafkaConsumerBootstrapServers(this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<KafkaConsumerBootstrapServerEnricher>();
        }

        public static LoggerConfiguration WithKafkaConsumerMayOffset(this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<KafkaConsumerKafkaConsumerMaxOffsetEnricher>();
        }
    }
}
