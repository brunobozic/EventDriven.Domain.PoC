using Confluent.Kafka;
using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Framework.Kafka.Core.DTOs.MessageProcessor;

using Serilog;
using SharedKernel.DomainContracts;
using SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedKernel.Kafka.ConsumedMessagePersistors
{
    public class ConsumedMessagePersistor : IConsumedMessagePersistor
    {
        private readonly IIntegrationEventPersistor _integrationEventPersistor;

        public ConsumedMessagePersistor(IIntegrationEventPersistor integrationEventPersistor)
        {
            _integrationEventPersistor = integrationEventPersistor;
        }

        public async Task<PersistingResult> PersistToInboxAsync(ConsumeMessageResult readResult)
        {
            Log
                .ForContext("MessageId", readResult.KafkaMessageId)
                .ForContext("MessageType", readResult.MessageType)
                .ForContext("Topic", readResult.Topic)
                .ForContext("Partition", readResult.Partition)
                .ForContext("CurrentOffset", readResult.CurrentOffset)
                .ForContext("ErrorMessage", readResult.ErrorMessage)
                .Information("Attempting message persistence of offset: [ {KafkaOffset} ] <{RequestMethod}>");

            var result = new PersistingResult();

            try
            {
                var messageType = Encoding.UTF8.GetString(readResult.CompleteMessage.Headers.GetLastBytes("MessageType"));
                var concreteType = ResolveType(messageType);

                if (concreteType == null)
                {
                    var errorMessage = $"Unknown message type: {messageType}";
                    Log.Error(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                var data = Deserialize(readResult.CompleteMessage.Message.Value, concreteType);
                if (data == null)
                {
                    var errorMessage = "Deserialization failed. The data is null.";
                    Log.Error(errorMessage);
                    throw new InvalidOperationException(errorMessage);
                }

                await _integrationEventPersistor.InsertIntoInbox((IIntegrationEvent<IDomainEvent>)data);

                if (!string.IsNullOrEmpty(readResult.ErrorMessage))
                {
                    readResult.Success = false;
                }

                if (!result.Success)
                {
                    Log
                       .ForContext("MessageId", readResult.KafkaMessageId)
                       .ForContext("MessageType", readResult.MessageType)
                       .ForContext("Topic", readResult.Topic)
                       .ForContext("Partition", readResult.Partition)
                       .ForContext("CurrentOffset", readResult.CurrentOffset)
                       .ForContext("ErrorMessage", readResult.ErrorMessage)
                       .Error("Message of offset: [ {KafkaOffset} ] <{RequestMethod}> **not** persisted, reason: [ " + result.Message + " ]");

                    Log
                       .ForContext("MessageId", readResult.KafkaMessageId)
                       .ForContext("MessageType", readResult.MessageType)
                       .ForContext("Topic", readResult.Topic)
                       .ForContext("Partition", readResult.Partition)
                       .ForContext("CurrentOffset", readResult.CurrentOffset)
                       .ForContext("ErrorMessage", readResult.ErrorMessage)
                       .Warning("Seeking offset: [ {KafkaOffset} ] <{RequestMethod}> re-reading the message.");
                }
            }
            catch (Exception ex)
            {
                Log
                    .ForContext("MessageId", readResult.KafkaMessageId)
                    .ForContext("MessageType", readResult.MessageType)
                    .ForContext("Topic", readResult.Topic)
                    .ForContext("Partition", readResult.Partition)
                    .ForContext("CurrentOffset", readResult.CurrentOffset)
                    .ForContext("ErrorMessage", readResult.ErrorMessage)
                    .Error("Message of offset: [ {KafkaOffset} ] <{RequestMethod}> **not** persisted, reason: [ " + ex.Message + " ]", ex);
                result.Success = false;
                result.Message = ex.Message;
            }

            if (!result.Success)
            {
                Log
                    .ForContext("MessageId", readResult.KafkaMessageId)
                    .ForContext("MessageType", readResult.MessageType)
                    .ForContext("Topic", readResult.Topic)
                    .ForContext("Partition", readResult.Partition)
                    .ForContext("CurrentOffset", readResult.CurrentOffset)
                    .ForContext("ErrorMessage", readResult.ErrorMessage)
                    .Error("Message of offset: [ {KafkaOffset} ] <{RequestMethod}> **not** persisted, reason: [ " + result.Message + " ]");

                Log
                    .ForContext("MessageId", readResult.KafkaMessageId)
                    .ForContext("MessageType", readResult.MessageType)
                    .ForContext("Topic", readResult.Topic)
                    .ForContext("Partition", readResult.Partition)
                    .ForContext("CurrentOffset", readResult.CurrentOffset)
                    .ForContext("ErrorMessage", readResult.ErrorMessage)
                    .Warning("Seeking offset: [ {KafkaOffset} ] <{RequestMethod}> re-reading the message.");
            }

            Log
                .ForContext("MessageId", readResult.KafkaMessageId)
                .ForContext("MessageType", readResult.MessageType)
                .ForContext("Topic", readResult.Topic)
                .ForContext("Partition", readResult.Partition)
                .ForContext("CurrentOffset", readResult.CurrentOffset)
                .ForContext("ErrorMessage", readResult.ErrorMessage)
                .Information("Message of offset: [ {KafkaOffset} ] <{RequestMethod}> persisted: [ " + result.Message + " ]");

            return result;
        }

        private Type ResolveType(string typeName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        private object Deserialize(string json, Type type)
        {
            var jsonDoc = JsonDocument.Parse(json);
            var rootElement = jsonDoc.RootElement;

            var ctor = type.GetConstructors().FirstOrDefault();
            var parameters = ctor.GetParameters();

            var args = new object[parameters.Length];
            foreach (var parameter in parameters)
            {
                if (rootElement.TryGetProperty(parameter.Name, out var jsonProperty))
                {
                    var value = JsonSerializer.Deserialize(jsonProperty.GetRawText(), parameter.ParameterType);
                    args[Array.IndexOf(parameters, parameter)] = value;
                }
            }

            var instance = Activator.CreateInstance(type, args);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (prop.CanWrite && rootElement.TryGetProperty(prop.Name, out var jsonProperty))
                {
                    var value = JsonSerializer.Deserialize(jsonProperty.GetRawText(), prop.PropertyType);
                    prop.SetValue(instance, value);
                }
            }

            return instance;
        }

        public bool MarkMessageAsFaulty(ConsumeMessageResult consumedMessage, string readStatusMessage)
        {
            return true;
        }

        public bool MarkMessageAsFaulty(string message, Error error, string errorReason, ConsumeResult<byte[], byte[]> consumerRecord)
        {
            return true;
        }
    }
}
