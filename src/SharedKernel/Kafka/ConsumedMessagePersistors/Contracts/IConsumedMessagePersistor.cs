using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Framework.Kafka.Core.DTOs.MessageProcessor;

namespace SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;

public interface IConsumedMessagePersistor
{
    /// <summary>
    ///     Handles message persistance (OracleDb)
    ///     The idea here is to save the appropriate message into the db, and commit kafka offset only if this does indeed
    ///     complete successfully.
    /// </summary>
    /// <param name="readResult"></param>
    /// <returns></returns>
    PersistingResult PersistToInbox(ConsumeMessageResult kafkaMessage);
}