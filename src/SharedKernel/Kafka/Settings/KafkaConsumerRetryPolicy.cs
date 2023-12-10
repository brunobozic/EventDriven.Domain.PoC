namespace SharedKernel.Kafka.Settings;

public class KafkaConsumerRetryPolicy
{
    public KafkaSharedExponentialBackoff ExponentialBackoff { get; set; }
    public int RetryTimes { get; set; }
}