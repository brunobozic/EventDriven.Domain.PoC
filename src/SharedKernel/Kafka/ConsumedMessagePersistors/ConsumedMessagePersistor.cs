using System;
using Confluent.Kafka;
using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Framework.Kafka.Core.DTOs.MessageProcessor;
using Serilog;
using SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;

namespace SharedKernel.Kafka.ConsumedMessagePersistors;

public class ConsumedMessagePersistor : IConsumedMessagePersistor
{
    public PersistingResult PersistToInbox(ConsumeMessageResult readResult)
    {
        Log
            .ForContext("MessageId", readResult.GadmMessageId)
            .Information("Attempting message persistence of offset: [ {KafkaOffset} ] <{RequestMethod}>");

        var r = new PersistingResult();

        try
        {
        }
        catch (Exception ex)
        {
            Log
                .ForContext("MessageId", readResult.MessageId)
                .Error(
                    "Message of offset: [ {KafkaOffset} ] <{RequestMethod}> **not** persisted, reason: [ " +
                    ex.Message + " ]", ex);
            r.Success = false;
            r.Message = ex.Message;
        }

        if (!r.Success)
        {
            Log
                .ForContext("MessageId", readResult.GadmMessageId)
                .Error("Message of offset: [ {KafkaOffset} ] <{RequestMethod}> **not** persisted, reason: [ " +
                       r.Message + " ]");

            // re-read the same message (we have not commited any offsets, but kafka will (in spite of this) proceed reading next message from topic because
            // consumers have an in-memory offset, so we need to "override" this...

            Log
                .ForContext("MessageId", readResult.GadmMessageId)
                .Warning("Seeking offset: [ {KafkaOffset} ] <{RequestMethod}> re-reading the message.");
        }

        Log
            .ForContext("MessageId", readResult.GadmMessageId)
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