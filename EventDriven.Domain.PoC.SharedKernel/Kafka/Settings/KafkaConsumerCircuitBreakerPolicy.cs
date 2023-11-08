namespace EventDriven.Domain.PoC.SharedKernel.Kafka.Settings
{
    public class KafkaConsumerCircuitBreakerPolicy
    {
        public int CooldownSeconds { get; set; }
        public int Tries { get; set; }
    }
}