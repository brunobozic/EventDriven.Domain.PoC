namespace EventDriven.Domain.PoC.SharedKernel.Kafka.Settings
{
    public class KafkaConsumerCircuitBreakerPolicy
    {
        public int Tries { get; set; }
        public int CooldownSeconds { get; set; }
    }
}