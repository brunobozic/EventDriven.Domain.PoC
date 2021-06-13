namespace EventDriven.Domain.PoC.SharedKernel.Kafka.Settings
{
    public class KafkaProducerCircuitBreakerPolicy
    {
        public int Tries { get; set; }
        public int CooldownSeconds { get; set; }
    }
}