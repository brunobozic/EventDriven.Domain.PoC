using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace Framework.Kafka.Core.Contracts
{
    public interface IKafkaScheduledProducer : IDisposable
    {
        Task WriteMessageAsync(string message);

        Handle UnderlyingHandle();

        IProducer<string, string> UnderlyingProducerInstance();
    }
}