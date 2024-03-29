﻿using System;
using Confluent.Kafka;
using EventDriven.Domain.PoC.SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;
using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Framework.Kafka.Core.DTOs.MessageProcessor;
using Serilog;

namespace EventDriven.Domain.PoC.SharedKernel.Kafka.ConsumedMessagePersistors
{
    public class ConsumedMessagePersistor : IConsumedMessagePersistor
    {
        public PersistingResult PersistToDb(ConsumeMessageResult readResult)
        {
            Log
                .ForContext("GADMMessageId", readResult.GadmMessageId)
                .Information("Attempting message persistence of offset: [ {KafkaOffset} ] <{GADMRequestMethod}>");

            var r = new PersistingResult();

            try
            {
            }
            catch (Exception ex)
            {
                Log
                    .ForContext("GADMMessageId", readResult.GadmMessageId)
                    .Error(
                        "Message of offset: [ {KafkaOffset} ] <{GADMRequestMethod}> **not** persisted, reason: [ " +
                        ex.Message + " ]", ex);
                r.Success = false;
                r.Message = ex.Message;
            }

            if (!r.Success)
            {
                Log
                    .ForContext("GADMMessageId", readResult.GadmMessageId)
                    .Error("Message of offset: [ {KafkaOffset} ] <{GADMRequestMethod}> **not** persisted, reason: [ " +
                           r.Message + " ]");

                // re-read the same message (we have not commited any offsets, but kafka will (in spite of this) proceed reading next message from topic because
                // consumers have an in-memory offset, so we need to "override" this...

                Log
                    .ForContext("GADMMessageId", readResult.GadmMessageId)
                    .Warning("Seeking offset: [ {KafkaOffset} ] <{GADMRequestMethod}> re-reading the message.");
            }

            Log
                .ForContext("GADMMessageId", readResult.GadmMessageId)
                .Information("Message of offset: [ {KafkaOffset} ] <{GADMRequestMethod}> persisted: [ " + r.Message +
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

        #region ctor

        #endregion ctor
    }
}