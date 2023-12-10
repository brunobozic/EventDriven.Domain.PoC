using System.Runtime.CompilerServices;
using Framework.Kafka.Core.Contracts;
using Serilog.Core;
using Serilog.Events;

namespace IdentityService.Api.Enrichers;

public class KafkaConsumerBootstrapServerEnricher : ILogEventEnricher
{
    public const string PropertyName = "KafkaConsumerBootstrapServer";
    private LogEventProperty _cachedProperty;

    public KafkaConsumerBootstrapServerEnricher()
    {
    }

    public KafkaConsumerBootstrapServerEnricher(IKafkaScheduledConsumer consumer)
    {
        _consumer = consumer;
    }

    public IKafkaScheduledConsumer _consumer { get; set; }

    /// <summary>
    ///     Enrich the log event.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
    }

    private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
    {
        //// Don't care about thread-safety, in the worst case the field gets overwritten and one property will be GCed
        //if (_consumer == null)
        //{
        //    _consumer = IoCContainer.Bootstrap.Container.Resolve<IKafkaScheduledConsumer>();
        //}

        if (_cachedProperty == null)
            _cachedProperty = CreateProperty(propertyFactory, _consumer);

        return _cachedProperty;
    }

    // Qualify as uncommon-path
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory,
        IKafkaScheduledConsumer consumer)
    {
        var value = consumer.GetBootstrapServers();
        return propertyFactory.CreateProperty(PropertyName, value);
    }
}