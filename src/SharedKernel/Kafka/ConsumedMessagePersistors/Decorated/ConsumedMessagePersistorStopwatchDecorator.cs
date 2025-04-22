using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Framework.Kafka.Core.DTOs.MessageProcessor;
using Serilog;
using SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SharedKernel.Kafka.ConsumedMessagePersistors.Decorated;

public class ConsumedMessagePersistorStopwatchDecorator : IConsumedMessagePersistor
{
    #region ctor

    public ConsumedMessagePersistorStopwatchDecorator(IConsumedMessagePersistor decorated)
    {
        _decorated = decorated;
    }

    #endregion ctor

    public IConsumedMessagePersistor _decorated { get; }

    public async Task<PersistingResult> PersistToInboxAsync(ConsumeMessageResult readResult)
    {
        Log.Information("Persisting message from kafka start [ {0} ]", DateTimeOffset.UtcNow);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var retVal = await _decorated.PersistToInboxAsync(readResult);

        stopwatch.Stop();

        Log.Information("Persisting message to database took: [ {0} ] Milliseconds", stopwatch.Elapsed.TotalMilliseconds);

        return retVal;
    }
}