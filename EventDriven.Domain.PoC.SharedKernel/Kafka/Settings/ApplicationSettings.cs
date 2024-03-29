﻿using EventDriven.Domain.PoC.SharedKernel.Kafka.Settings.Contracts;
using Framework.Kafka.Core.KafkaSettings;

namespace EventDriven.Domain.PoC.SharedKernel.Kafka.Settings
{
    public class ApplicationSettings : IApplicationSettings
    {
        public string WorkerName { get; set; }

        public int? MainKafkaMessagePollInterval { get; set; }
        public string Environment { get; set; }
        public KafkaConsumerSettings KafkaConsumerSettings { get; set; }
        public KafkaProducerSettings KafkaProducerSettings { get; set; }
        public PollySettings PollySettings { get; set; }

        public DeadLetterArchiveJobSettings DeadLetterArchiveJobSettings { get; set; }

        public DeadLetterOutboxJobSettings DeadLetterOutboxJobSettings { get; set; }
        public KafkaLoggingProducerSettings KafkaLoggingProducerSettings { get; set; }
    }
}