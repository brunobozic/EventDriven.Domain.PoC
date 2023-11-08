using Framework.Kafka.Core.KafkaSettings;

namespace EventDriven.Domain.PoC.SharedKernel.Kafka.Settings.Contracts
{
    public interface IApplicationSettings
    {
        DeadLetterArchiveJobSettings DeadLetterArchiveJobSettings { get; set; }
        DeadLetterOutboxJobSettings DeadLetterOutboxJobSettings { get; set; }
        string Environment { get; set; }
        KafkaConsumerSettings KafkaConsumerSettings { get; set; }
        KafkaLoggingProducerSettings KafkaLoggingProducerSettings { get; set; }
        KafkaProducerSettings KafkaProducerSettings { get; set; }
        PollySettings PollySettings { get; set; }
    }
}