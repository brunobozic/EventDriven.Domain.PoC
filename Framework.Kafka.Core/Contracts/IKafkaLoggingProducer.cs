using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace Framework.Kafka.Core.Contracts
{
    public interface IKafkaLoggingProducer : IDisposable
    {
        Task WriteLogMessageAsync(string message);

        Handle UnderlyingHandle();

        IProducer<string, string> UnderlyingProducerInstance();
    }
}