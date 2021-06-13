namespace Framework.Kafka.Core.KafkaSettings
{
    public class KafkaLoggingProducerSettings : IKafkaLoggingProducerSettings
    {
        public string GroupId { get; set; }
        public string AutoOffsetReset { get; set; }
        public bool EnableAutoOffsetStore { get; set; }
        public string KafkaTopic { get; set; }

        public string BootstrapServers { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
        public string SaslMechanism { get; set; }

        public string SecurityProtocol { get; set; }
        public string ClientId { get; set; }
        public bool? Debug { get; set; }
    }
}