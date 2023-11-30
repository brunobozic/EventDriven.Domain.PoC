using Confluent.Kafka;
using EventDriven.Domain.PoC.SharedKernel.Kafka.ConsumedMessagePersistors.Contracts;
using EventDriven.Domain.PoC.SharedKernel.Kafka.Settings;
using Framework.Kafka.Core.Contracts;
using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Framework.Kafka.Core.DTOs.MessageProcessor;
using Quartz;
using System.Collections.Generic;
using System;
using Polly.Contrib.WaitAndRetry;
using Polly;
using Serilog;

namespace EventDriven.Domain.PoC.Api.Rest.QuartzJobs
{
    public class KafkaPollJobController : IJobController
    {
        #region ctor

        public KafkaPollJobController(
            ApplicationSettings settings
            , IKafkaScheduledConsumer kafkaScheduledConsumer
            , IConsumedMessagePersistor messagePersistor
        )
        {
            _settings = settings;
            _c = kafkaScheduledConsumer;
            _messagePersistor = messagePersistor;
        }

        #endregion ctor

        #region Public methods

        public void ReadAndProcessKafkaMessage(JobDataMap jobDataMap)
        {
            ConsumeMessageResult kafkaMessage = null;
            PersistingResult persistResult = null;
            PersistingResult resultOfPersisting = new PersistingResult();

            Log.Information("Topic: [ {KafkaConsumerTopic} ]" + ", partition: " + _c.Instance().Assignment[0].Partition);

            // Kafka will retry producing a message by itself, given the defined produce interval is large enough
            // Kafka will retry consuming a message by itself, given the defined consume interval is large enough
            // since I needed both retrying and circuit breaking (and skipping poison messages) I opted to
            // delegate some of those concerns to "Polly" - due to somewhat better control Polly offers
            // in order to facilitate Polly - I had to wrap the Kafka consumed message in a wrapper thru which I was then able to control exceptions
            // that happen in Kafka, this is necessary because these returned messages are used as input arguments for Polly,
            // Polly decides when to retry/circuit break after it evaluates whether the kafka message was read successfully or not.
            // Offset will be commited *manually*, when the **entire** process finishes successfully (and only if successfull, but also see *poison pill*)

            // define the exponential back-off
            var delay = Backoff.ExponentialBackoff(TimeSpan.FromMilliseconds(_settings.PollySettings.KafkaSharedExponentialBackoff.InitialDelayMiliseconds)
                , _settings.PollySettings.KafkaConsumerCircuitBreakerPolicy.Tries);

            // create the retry policy with exponential back-off
            // we need to create a situation where kafkaMessage reading is attempted n times (with exponential backoff) and
            // if an exception still happens after n retry attempts, the kafkaMessage gets skipped
            var retryPolicy = Policy
                .HandleResult<ConsumeMessageResult>(r => !r.Success && !r.ErrorMessage.Contains("No new messages") && !r.ErrorMessage.Contains("No offset stored"))
                .WaitAndRetry(delay, (response, retryCount) =>
                {
                    kafkaMessage = response.Result;

                    // unfortunately, kafka might return an invalid kafkaMessage, therefore in some cases we wont have any additional (diagnostic) information to pursue
                    if (kafkaMessage != null)
                    {
                        // seek to the last seen position (offset) to enable re-reading of the same kafkaMessage that had previously failed
                        // if we dont do this, Kafka will try to read the *next* message in line because the consumer instance keeps its own track of which messages
                        // it has seen (irrespective of the comitted offset) and will normally try to read the *next* message in line
                        if (kafkaMessage.ErrorMessage != "IsPartitionEOF")
                        {
                            RereadTheSame(kafkaMessage);

                            Log
                               .ForContext("Retry", retryCount)
                               .ForContext("ErrorMessage", response.Result.ErrorMessage)
                               .ForContext("Offset", response.Result.Offset)
                               .ForContext("Partition", response.Result.Partition)
                               .Warning(" => retry count: [ {Retry} ], Offset: [ {Offset} ], partition: [ {Partition} ], Message reading topic: [ {KafkaConsumerTopic} ], retry reason: {ErrorMessage}");
                        }
                    }
                    else // not enough info to do anything but log
                    {
                        Log.ForContext("Retry", retryCount)
                           .ForContext("ErrorMessage", response.Result.ErrorMessage)
                           .Warning(" => retry **failed**, Offset: [ {KafkaOffset} ], topic: [ {KafkaConsumerTopic} ], retry reason: {ErrorMessage}");
                    }
                });

            /// =========================================================
            /// ===========         Message Reading        ==============
            /// =========================================================
            kafkaMessage = retryPolicy.Execute(() => _c.Consume());

            // kafkaMessage reading was attempted n times and it has either succeeded or failed,
            // if it is a fail then we need to *skip* the faulted kafka message
            if (kafkaMessage.Success)
            {
                // if reading the kafkaMessage was a *success*, process the kafkaMessage (in other words => persist it into donat database)
                // if persistance is successfull, commit the ofset (remember, we are *manually* commiting the offset, so that we can ensure that the entire
                // process is completed successfully before commiting),
                // otherwise *do not* commit the offset and skip the message after n failed attempts to persist it

                #region Process the message and commit offset if processing was successfull

                // this policy will make n retry calls to the perister, after which if the result is still a fail the message will get skipped
                // if the Oracle data store returns a "IsFatal" mark, we will not be making any retry attempts because this marker is used to tell us
                // that persistance is futile :)
                var persistorRetryPolicy = Policy
                    .HandleResult<PersistingResult>(r => !r.Success && !r.IsFatal)
                    .WaitAndRetry(delay, (response, retryCount) =>
                    {
                        persistResult = response.Result;
                        Log
                           .ForContext("RetryCount", retryCount)
                           .ForContext("Offset", persistResult.Offset)
                           .ForContext("Partition", persistResult.Partition)
                           
                           .ForContext("GADMMessageId", kafkaMessage.GadmMessageId)
                           .Warning("Persistor retry count: [ {RetryCount} ] => " + "offset: [ {KafkaOffset} ] <{GADMRequestMethod}>" + Environment.NewLine + "      ===> Retry reason: " + response.Result.Message + " ]");
                    });

                /// =========================================================
                /// ===========       Message Persisting       ==============
                /// =========================================================
                resultOfPersisting = persistorRetryPolicy.Execute(() => _messagePersistor.PersistToDb(kafkaMessage));

                if (resultOfPersisting.Success)
                {
                    try
                    {
                        /// =========================================================
                        /// ===========         Storing offset         ==============
                        /// =========================================================
                        _c.StoreOffsetFor(kafkaMessage.CompleteMessage);
                    }
                    catch (Exception offsetStoreException)
                    {
                        Log.ForContext("Offset", kafkaMessage.Offset)
                           .ForContext("Partition", kafkaMessage.Partition)
                      
                           .ForContext("GADMMessageId", kafkaMessage.GadmMessageId)
                           .Error("Message of offset: [ {Offset} ], partition: [ {Partition} ] <{GADMRequestMethod}> not persisted, reason: [ " + offsetStoreException.Message + " ]", offsetStoreException);
                    }
                }
                else // persistance has failed
                {
                    /// =========================================================
                    /// ===========        Message skipping        ==============
                    /// =========================================================
                    SkipProblematic(kafkaMessage);
                }

                #endregion Process the message and commit offset if processing was successfull
            }
            else // Message was **NOT** read from kafka!!! unfortunately, this might as well mean that we get no information on what the current offset might be
            {
                // if the reason the last read attempt failed was - no more messages on Kafka, return immediately
                if (kafkaMessage.ErrorMessage.Contains("EOF")) { return; }

                #region Dead letter queue and message skipping

                Log.ForContext("Offset", kafkaMessage.Offset)
                   .ForContext("Partition", kafkaMessage.Partition)
                   .Information("Message **not** read from offset: [ {Offset} ], partition [ {Partition} ], topic: [ {KafkaConsumerTopic} ], found [ " + _c.Instance().Assignment.Count + " ] assignments, brokers: [ {KafkaConsumerBootstrapServer} ]");

                // WARNING! consumer assignment might be null at this point if the coordinator allready unsubscribed our consumer due to a heartbeat/poll timeout!
                // it may also be null due to non-consumer exception (general exception or a "broker down" exception)
                if (_c.Instance().Assignment.Count > 0)
                {
                    if (!kafkaMessage.ErrorMessage.Contains("Local: No offset stored") && !kafkaMessage.ErrorMessage.Contains("No new messages"))
                    {
                        NotifyProblemWith(kafkaMessage);

                        #region Send to DLQ

                        Log.ForContext("Partition", kafkaMessage.Partition)
                           .ForContext("Topic", kafkaMessage.Topic)
                           .ForContext("Offset", kafkaMessage.Offset)
                           .Information("Message at offset: [ {Offset} ], partition: [ {Partition} ], topic: [ {Topic} ] will be skipped, sending to dead letter queue, brokers: [ {KafkaConsumerBootstrapServer} ]");

                        /// =========================================================
                        /// ===========       Dead Letter Queue        ==============
                        /// =========================================================

                        // TODO!
                        Log.ForContext("Partition", kafkaMessage.Partition)
                           .ForContext("Topic", kafkaMessage.Topic)
                           .ForContext("Offset", kafkaMessage.Offset)
                           .Warning("Message at offset: [ {Offset} ], partition: [ {Partition} ], topic: [ {Topic} ]  sent to dead letter queue, brokers: [ {KafkaConsumerBootstrapServer} ]");

                        #endregion Send to DLQ

                        /// =========================================================
                        /// ===========        Message skipping        ==============
                        /// =========================================================
                        SkipProblematic(kafkaMessage);
                    }
                    else
                    {
                        // _c.Seek(new TopicPartition(kafkaMessage.Topic, kafkaMessage.Partition), kafkaMessage.Offset);
                        // Log.Fatal("Unable to skip the kafkaMessage of unknown offset.");
                    }
                }
                else
                {
                    Log.Warning("No new messages were found (possible EOF), brokers: [ {KafkaConsumerBootstrapServer} ]");
                }

                #endregion Dead letter queue and message skipping
            }
        }

        /// <summary>
        /// Will skip the "poison pill" by taking the last offset, incrementing it by 1 and seek-ing to *that* position
        /// Effectively this will mean - read the next message, forget about the current one.
        /// </summary>
        /// <param name="kafkaMessage"></param>
        private void SkipProblematic(ConsumeMessageResult kafkaMessage)
        {
            try
            {
                _c.SkipPoisonPill(new TopicPartition(_c.GetTopic(), kafkaMessage.Partition), _c.Instance().Position(new TopicPartition(_c.GetTopic(), kafkaMessage.Partition)).Value);
            }
            catch (Exception skippingException)
            {
                Log
                    .ForContext("Offset", kafkaMessage.Offset)
                    .ForContext("Partition", kafkaMessage.Partition)
                    .Fatal("Message **not** skipped, offset: [ {Offset} ], partition [ {Partition} ], topic: [ {KafkaConsumerTopic} ], found [ " + _c.Instance().Assignment.Count + " ] assignments, brokers: [ {KafkaConsumerBootstrapServer} ]", skippingException);
            }
        }

        #endregion Public methods

        #region Private methods

        private void NotifyProblemWith(ConsumeMessageResult consumedMessage)
        {
            var readStatusMessage = "Message processing failed after [ " + _settings.PollySettings.KafkaConsumerRetryPolicy.RetryTimes +
                                    " ] retry attempts => (Id: [ " + consumedMessage.GadmMessageId +
                                    " ], Payload: [ " + consumedMessage.Message +
                                    " ]), reason [ " + consumedMessage.ErrorMessage;

            Log.Error(readStatusMessage + ", at offset: [ {KafkaOffset} ]" + ", Broker: [ {KafkaConsumerBootstrapServer} ]");
        }

        private void RereadTheSame(ConsumeMessageResult message)
        {
            TopicPartition topicPartition = new TopicPartition(_c.GetTopic(), new Partition(message.Partition));
            _c.Instance().Assign(new List<TopicPartitionOffset> { new TopicPartitionOffset(topicPartition, message.Offset) });

            Log.ForContext("Offset", message.Offset)
               .ForContext("Partition", message.Partition)
               .Warning("Rereading the message, setting offset position to: [ {Offset} ], partition: [ {Partition} ] " + ", Broker: [ {KafkaConsumerBootstrapServer} ]");
        }
        #endregion Private methods

        #region Private props

        private readonly IKafkaScheduledConsumer _c;
        private readonly IConsumedMessagePersistor _messagePersistor;
        private readonly ApplicationSettings _settings;
        #endregion Private props
    }
}
