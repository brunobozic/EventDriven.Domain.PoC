namespace EventDriven.Domain.PoC.SharedKernel.Kafka.Settings
{
    public class KafkaProducerRetryPolicy
    {
        public int RetryTimes { get; set; }
        public KafkaSharedExponentialBackoff ExponentialBackoff { get; set; }
    }
}