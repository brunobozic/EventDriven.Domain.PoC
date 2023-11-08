namespace EventDriven.Domain.PoC.SharedKernel.Kafka.Settings
{
    public class KafkaProducerRetryPolicy
    {
        public KafkaSharedExponentialBackoff ExponentialBackoff { get; set; }
        public int RetryTimes { get; set; }
    }
}