namespace SharedKernel.Kafka.Settings;

public class KafkaProducerCircuitBreakerPolicy
{
    public int CooldownSeconds { get; set; }
    public int Tries { get; set; }
}