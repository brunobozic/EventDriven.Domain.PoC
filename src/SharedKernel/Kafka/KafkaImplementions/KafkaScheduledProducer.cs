using Confluent.Kafka;
using Framework.Kafka.Core.Contracts;
using Framework.Kafka.Core.DTOs.KafkaProducer;
using Serilog;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Kafka.KafkaImplementions;

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

    public async Task<bool> WriteMessageAsync(string messageType, string messageData)
    {
        var headers = new Headers();
        headers.Add("MessageType", Encoding.UTF8.GetBytes(messageType));

        var message = new Message<string, string>
        {
            Key = new Random().Next(5).ToString(),
            Value = messageData,
            Headers = headers
        };

        var result = await _producer.ProduceAsync(_topicName, message).ContinueWith(task =>
            task.IsFaulted
                ? $"Error producing message: {task.Exception.Message}"
                : $"Produced to: {task.Result.TopicPartitionOffset}");

        _producer.Flush(TimeSpan.FromSeconds(10));

        return !result.StartsWith("Error");
    }
}