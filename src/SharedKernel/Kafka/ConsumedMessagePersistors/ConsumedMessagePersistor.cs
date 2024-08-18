using Confluent.Kafka;
using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Framework.Kafka.Core.DTOs.MessageProcessor;
using IdentityService.Application.CQRSBoilerplate.OutboxCommands;
using Newtonsoft.Json;
using Serilog;
using SharedKernel.DomainContracts;
using SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;
using System;

namespace SharedKernel.Kafka.ConsumedMessagePersistors;

public class ConsumedMessagePersistor : IConsumedMessagePersistor
{
    private readonly IIntegrationEventPersistor _integrationEventPersistor;

    public ConsumedMessagePersistor(IIntegrationEventPersistor integrationEventPersistor)
    {
        _integrationEventPersistor = integrationEventPersistor;
    }

    public PersistingResult PersistToInbox(ConsumeMessageResult readResult)
    {
        Log
            .ForContext("MessageId", readResult.KafkaMessageId)
            .Information("Attempting message persistence of offset: [ {KafkaOffset} ] <{RequestMethod}>");

        var r = new PersistingResult();

        try
        {
            if (!string.IsNullOrEmpty(readResult.ErrorMessage)) { readResult.Success = false; }

            if (!r.Success)
            {
                Log
                   .ForContext("MessageId", readResult.KafkaMessageId)
                   .Error("Message of offset: [ {KafkaOffset} ] <{RequestMethod}> **not** persisted, reason: [ " +
                       r.Message + " ]");

                // re-read the same message (we have not commited any offsets, but kafka will (in spite of this) proceed reading next message from topic because
                // consumers have an in-memory offset, so we need to "override" this...

                Log
                   .ForContext("MessageId", readResult.KafkaMessageId)
                   .Warning("Seeking offset: [ {KafkaOffset} ] <{RequestMethod}> re-reading the message.");

                return r;
            }
            else
            {
                var data = JsonConvert.DeserializeObject<IIntegrationEvent<IDomainEvent>>(readResult.CompleteMessage.Message.Value);
                _integrationEventPersistor.InsertIntoInbox(data);
            }

        }
        catch (Exception ex)
        {
            Log
                .ForContext("MessageId", readResult.KafkaMessageId)
                .Error(
                    "Message of offset: [ {KafkaOffset} ] <{RequestMethod}> **not** persisted, reason: [ " +
                    ex.Message + " ]", ex);
            r.Success = false;
            r.Message = ex.Message;
        }

        if (!r.Success)
        {
            Log
                .ForContext("MessageId", readResult.KafkaMessageId)
                .Error("Message of offset: [ {KafkaOffset} ] <{RequestMethod}> **not** persisted, reason: [ " +
                       r.Message + " ]");

            // re-read the same message (we have not commited any offsets, but kafka will (in spite of this) proceed reading next message from topic because
            // consumers have an in-memory offset, so we need to "override" this...

            Log
                .ForContext("MessageId", readResult.KafkaMessageId)
                .Warning("Seeking offset: [ {KafkaOffset} ] <{RequestMethod}> re-reading the message.");
        }

        Log
            .ForContext("MessageId", readResult.KafkaMessageId)
            .Information("Message of offset: [ {KafkaOffset} ] <{RequestMethod}> persisted: [ " + r.Message +
                         " ]");

        return r;
    }

    public bool MarkMessageAsFaulty(ConsumeMessageResult consumedMessage, string readStatusMessage)
    {
        return true;
    }

    public bool MarkMessageAsFaulty(string message, Error error, string errorReason,
        ConsumeResult<byte[], byte[]> consumerRecord)
    {
        return true;
    }
}