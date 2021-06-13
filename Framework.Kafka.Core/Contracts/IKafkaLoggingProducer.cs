using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace Framework.Kafka.Core.Contracts
{
    public interface IKafkaLoggingProducer : IDisposable
    {
        Task WriteLogMessageAsync(string message);

        Handle UnderlyingHandle();

        IProducer<string, string> UnderlyingProducerInstance();
    }
}