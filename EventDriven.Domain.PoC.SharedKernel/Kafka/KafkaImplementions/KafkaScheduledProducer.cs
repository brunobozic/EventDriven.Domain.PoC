using Confluent.Kafka;
using Framework.Kafka.Core.Contracts;
using Framework.Kafka.Core.DTOs.KafkaProducer;
using Serilog;
using System;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.SharedKernel.Kafka.KafkaImplementions
{
    public class KafkaScheduledProducer : IKafkaScheduledProducer
    {
        private static readonly Random rand = new();
        private readonly ProducerConfig _config;
        private readonly IProducer<string, string> _producer;
        private readonly string _topicName;

        public KafkaScheduledProducer(ProducerConfig config, string topicName)
        {
            _topicName = topicName;
            _config = config;
            MessageProducingResult messageProducingResult;
            try
            {
                _producer = new ProducerBuilder<string, string>(_config).Build();
            }
            catch (Exception producingEx)
            {
                Log.Error(
                    "Message consumer failed, reason [ " + producingEx.Message +
                    " ] was not committed, the message will be read again.", producingEx);

                messageProducingResult = new MessageProducingResult
                {
                    ProducedStatusMessage = "Message producer failed, reason [ " + producingEx.Message + " ] ",
                    ErrorType = "produce"
                };
            }
        }

        public async Task<bool> WriteMessageAsync(string message)
        {
            var successfullDelivery = await _producer.ProduceAsync(_topicName,
                    new Message<string, string> { Key = rand.Next(5).ToString(), Value = message })
                .ContinueWith(task => task.IsFaulted
                    ? $"error producing message: {task.Exception.Message}"
                    : $"produced to: {task.Result.TopicPartitionOffset}");

            // block until all in-flight produce requests have completed (successfully
            // or otherwise) or 10s has elapsed.
            _producer.Flush(TimeSpan.FromSeconds(10));

            return true;
        }

        public void Dispose()
        {
            _producer?.Flush();
            _producer?.Dispose();
        }

        public Handle UnderlyingHandle()
        {
            return _producer != null ? _producer.Handle : null;
        }

        public IProducer<string, string> UnderlyingProducerInstance()
        {
            return _producer;
        }
    }
}