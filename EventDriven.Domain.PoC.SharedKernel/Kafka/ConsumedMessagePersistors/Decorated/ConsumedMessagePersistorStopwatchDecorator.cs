using System;
using System.Diagnostics;
using EventDriven.Domain.PoC.SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;
using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Framework.Kafka.Core.DTOs.MessageProcessor;
using Serilog;

namespace EventDriven.Domain.PoC.SharedKernel.Kafka.ConsumedMessagePersistors.Decorated
{
    public class ConsumedMessagePersistorStopwatchDecorator : IConsumedMessagePersistor
    {
        #region ctor

        public ConsumedMessagePersistorStopwatchDecorator(IConsumedMessagePersistor decorated)
        {
            _decorated = decorated;
        }

        #endregion ctor

        public IConsumedMessagePersistor _decorated { get; }

        public PersistingResult PersistToDb(ConsumeMessageResult readResult)
        {
            Log.Information("Persisting message from kafka start [ {0} ]", DateTimeOffset.UtcNow);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var retVal = _decorated.PersistToDb(readResult);
            stopwatch.Stop();
            Log.Information("Persisting message to Donat database took: [ {0} ] Milliseconds",
                stopwatch.Elapsed.TotalMilliseconds);

            return retVal;
        }
    }
}