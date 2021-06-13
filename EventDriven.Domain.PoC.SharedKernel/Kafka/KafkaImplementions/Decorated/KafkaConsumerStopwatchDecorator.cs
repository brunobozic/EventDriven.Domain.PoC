using System;
using System.Collections.Generic;
using System.Diagnostics;
using Confluent.Kafka;
using Framework.Kafka.Core.Contracts;
using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Serilog;

namespace EventDriven.Domain.PoC.SharedKernel.Kafka.KafkaImplementions.Decorated
{
    public class KafkaConsumerStopwatchDecorator : IKafkaScheduledConsumer
    {
        #region Private Props

        private readonly IKafkaScheduledConsumer _decorated;

        #endregion Private Props

        #region ctor

        public KafkaConsumerStopwatchDecorator(IKafkaScheduledConsumer consumer)
        {
            _decorated = consumer;
        }

        #endregion ctor

        public void Continue()
        {
            _decorated.Continue();
        }

        public void Dispose()
        {
            _decorated.Dispose();
        }

        public string GetBootstrapServers()
        {
            return _decorated.GetBootstrapServers();
        }

        public string GetCurrentOffset()
        {
            return _decorated.GetCurrentOffset();
        }

        public string GetKafkaConsumerMaxOffset()
        {
            return _decorated.GetKafkaConsumerMaxOffset();
        }

        public string GetTopic()
        {
            return _decorated.GetTopic();
        }

        public IConsumer<string, string> Instance()
        {
            return _decorated.Instance();
        }

        public void Pause()
        {
            _decorated.Pause();
        }

        /// <summary>
        ///     Reads a single message from Kafka topic/partition the consumer is subscribed to.
        ///     Crucial information here is the _lastOffset variable, we store the consumed message offset into it (if available).
        /// </summary>
        /// <returns></returns>
        public ConsumeMessageResult Consume()
        {
            Log.Information("Reading message from kafka start [ {0} ]", DateTimeOffset.UtcNow);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var retVal = _decorated.Consume();
            stopwatch.Stop();
            Log.Information("Reading message from kafka took: [ {0} ] Milliseconds",
                stopwatch.Elapsed.TotalMilliseconds);
            return retVal;
        }

        public void Seek(TopicPartition topicPartition, long recordOffset)
        {
            _decorated.Seek(topicPartition, recordOffset);
        }

        public bool SkipPoisonPill(ConsumeResult<string, string> consumedMessage)
        {
            return _decorated.SkipPoisonPill(consumedMessage);
        }

        public bool SkipPoisonPill(TopicPartition topicPartition, long recordOffset)
        {
            return _decorated.SkipPoisonPill(topicPartition, recordOffset);
        }

        public void StoreOffsetFor(ConsumeResult<string, string> msg)
        {
            _decorated.StoreOffsetFor(msg);
        }

        public Handle UnderlyingHandle()
        {
            return _decorated.UnderlyingHandle();
        }

        public List<string> UnderlyingSubscriptions()
        {
            return _decorated.UnderlyingSubscriptions();
        }
    }
}