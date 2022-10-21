using Confluent.Kafka;
using Framework.Kafka.Core.Contracts;
using Framework.Kafka.Core.DTOs.KafkaConsumer;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventDriven.Domain.PoC.SharedKernel.Kafka.KafkaImplementions
{
    public class KafkaScheduledConsumer : IKafkaScheduledConsumer
    {
        #region ctor

        /// <summary>
        ///     Will create a new Kafka consumer with provided configuration, and will subscribe to the provided topic name,
        ///     all-in-one
        ///     Overrides contained within.
        ///     IDisposable.
        /// </summary>
        /// <param name="messageProcessor"></param>
        /// <param name="config"></param>
        /// <param name="topicName"></param>
        public KafkaScheduledConsumer(
            ConsumerConfig consumerconfig
            , string topicName
        )
        {
            if (string.IsNullOrEmpty(topicName)) throw new ArgumentNullException(nameof(topicName));
            _config = consumerconfig ?? throw new ArgumentNullException(nameof(consumerconfig));
            _topicName = topicName;
            _c = new ConsumerBuilder<string, string>(_config).Build();
            _c.Subscribe(_topicName);
        }

        #endregion ctor

        #region IDisposable

        public void Dispose()
        {
            if (_c == null) return;
            _c.Unsubscribe();
            _c.Close(); // will commit offset, *IMPORTANT*=> will also rebalance the groups/consumers/partition assignments  <=*IMPORTANT*
            // (this is why we are using singleton),
            // must be called *before* calling dispose

            _c.Dispose();
        }

        #endregion IDisposable

        #region Public Properties

        public IConsumer<string, string> Instance()
        {
            return _c;
        }

        public readonly TopicPartitionOffset CurrentTopicPartitionOffset;

        #endregion Public Properties

        #region Private Props

        private readonly string _topicName;
        private readonly IConsumer<string, string> _c;
        private int _currentPartition;
        private ConsumeResult<string, string> _consumedMessage;

        // ReSharper disable once NotAccessedField.Local
        private readonly ConsumerConfig _config;

        private long _currentOffset { get; set; }
        private long _lastOffset { get; set; }
        private long _needsToHitOffset { get; set; }
        private TopicPartition _needsToHitTopicPartition { get; set; }

        #endregion Private Props

        #region Public methods

        /// <summary>
        ///     Reads a single message from Kafka topic/partition the consumer is subscribed to.
        ///     Crucial information here is the _lastOffset variable, we store the consumed message offset into it (if available).
        /// </summary>
        /// <returns></returns>
        public ConsumeMessageResult Consume()
        {
            ConsumeMessageResult messageConsumingResult;

            try
            {
                messageConsumingResult = ConsumeMessage();

                // this is the last successfully read offset, that at this point is still not commited
                // IMPORTANT => last commited offset may or may not be equal to the last offset read from the topic/partition
                if (!messageConsumingResult.ErrorMessage.ToUpper().Contains("EOF"))
                    _lastOffset = messageConsumingResult.Offset;

                ParseAndLog(messageConsumingResult);

                return messageConsumingResult;
            }
            catch (ConsumeException cex)
            {
                if (cex.Message.Contains("No offset stored"))
                {
                    Log.Error("Message consumer failed, reason [ " + cex.Message + " ]", cex);

                    messageConsumingResult = new ConsumeMessageResult
                    {
                        ErrorMessage = "Message consumer failed, reason [ " + cex.Message + " ]",
                        ErrorType = "nooffset"
                    };
                }
                else
                {
                    Log.Error(
                        "Message consumer failed, reason [ " + cex.Message + " ], offset [ " + _currentOffset + " ]",
                        cex);

                    messageConsumingResult = new ConsumeMessageResult
                    {
                        ErrorMessage = "Message consumer failed, reason [ " + cex.Message + " ], offset [ " +
                                       _currentOffset + " ]",
                        ErrorType = "consume",
                        // this is important - we need to read the last offset read and store into a variable
                        Offset = _currentOffset,
                        Partition = _currentPartition
                    };
                }
            }
            catch (Exception genericException)
            {
                Log.Logger.Error(genericException.Message, genericException);

                messageConsumingResult = new ConsumeMessageResult
                {
                    ErrorMessage = "Message consumer failed, reason [ " + genericException.Message + " ], offset [ " +
                                   _currentOffset + " ]",
                    ErrorType = "general",
                    // this is important - we need to read the last offset read and store into a variable
                    Offset = _currentOffset,
                    Partition = _currentPartition
                };
            }

            if (!messageConsumingResult.ErrorMessage.ToUpper().Contains("EOF"))
                _lastOffset = messageConsumingResult.Offset;

            return messageConsumingResult;
        }

        /// <summary>
        ///     Returns the underlying consumer instance.
        /// </summary>
        /// <returns></returns>
        public Handle UnderlyingHandle()
        {
            return _c != null ? _c.Handle : null;
        }

        /// <summary>
        ///     Returns a list of consumer subscriptions.
        /// </summary>
        /// <returns></returns>
        public List<string> UnderlyingSubscriptions()
        {
            return _c != null ? _c.Subscription : null;
        }

        public void Pause()
        {
            // _c.Pause();
        }

        public void Continue()
        {
            // _c.Resume(?);
        }

        /// <summary>
        ///     Returns the topic we have subscribed our consumer to.
        /// </summary>
        /// <returns></returns>
        public string GetTopic()
        {
            return _topicName;
        }

        /// <summary>
        ///     Returns the latest offset that we wish to track.
        ///     The returned value will not be equal to last commited offset nor the current consumer tracked offset
        ///     Instead this will return the last offset we declared as our target offset.
        ///     This is due to enabling multiple consecutive message skipping. No logging.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentOffset()
        {
            var sb = new StringBuilder();

            if (_c.Assignment.Count > 0)
            {
                var counter = 0;
                sb.AppendFormat("P/Offset: ");
                foreach (var item in _c.Assignment)
                {
                    var offset = _c.Position(new TopicPartition(item.Topic, item.Partition));
                    if (offset.Value != -1001)
                    {
                        sb.AppendFormat("[{0}{1}", item.Partition.Value, "/");
                        sb.AppendFormat("{0}]", offset.Value);
                        if (counter != _c.Assignment.Count - 1)
                            sb.AppendFormat("{0}", ",");
                    }

                    counter = counter + 1;
                }
            }

            return sb.ToString();
        }

        public bool SkipPoisonPill(ConsumeResult<string, string> consumedMessage)
        {
            if (consumedMessage != null)
            {
                var o = new Offset(consumedMessage.Offset.Value + 1);
                var tpo = new TopicPartitionOffset(consumedMessage.TopicPartition, o);

                _c.Seek(tpo);

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Skips a message by seeking to one position ahead.
        ///     Sets the needsToHitOffset to the targeted position to enable multiple consecutive message skipping.
        ///     Handles its own logging.
        ///     Returns false if fails.
        /// </summary>
        /// <param name="topicPartition"></param>
        /// <param name="recordOffset"></param>
        /// <returns></returns>
        public bool SkipPoisonPill(TopicPartition topicPartition, long recordOffset)
        {
            Log.ForContext("Partition", topicPartition.Partition.Value)
                .ForContext("Topic", topicPartition.Topic)
                .ForContext("Offset", _lastOffset)
                .Warning(
                    "Message at offset: [ {Offset} ], partition: [ {Partition} ], topic: [ {Topic} ] is to be skipped");

            // we need to track what offset was last stored, what offset we are currently at, and - what offset we need to hit
            // each needs to be tracked separately
            // in this case we are setting the offset that we would like to hit as _needsToHitOffset marker
            // this is needed because we might be skipping more than 1 consecutive message, and in this case we must not track
            // last commited offset nor current offset, instead we need to make sure we are tracking the _needsToHitOffset marker
            _needsToHitOffset = _lastOffset + 1;
            // messages can belong to different partitions...
            _needsToHitTopicPartition = topicPartition;

            var targetOffset = new TopicPartitionOffset(_needsToHitTopicPartition, _needsToHitOffset);

            try
            {
                _c.Seek(targetOffset);
            }
            catch (Exception ex)
            {
                Log.ForContext("Partition", targetOffset.Partition.Value)
                    .ForContext("Topic", targetOffset.Topic)
                    .ForContext("Offset", _lastOffset)
                    .Fatal(
                        "Message at offset: [ {Offset} ], partition: [ {Partition} ], topic: [ {Topic} ] was **not** skipped",
                        ex);

                return false;
            }

            Log.ForContext("Partition", targetOffset.Partition.Value)
                .ForContext("Topic", targetOffset.Topic)
                .ForContext("Offset", _lastOffset)
                .Warning("Message at offset: [ {Offset} ], partition: [ {Partition} ], topic: [ {Topic} ] was skipped");

            // resetting this to 0 otherwise it will keep seeking to this position when theres no need!
            _needsToHitOffset = 0;

            return true;
        }

        public void StoreOffsetFor(ConsumeResult<string, string> msg)
        {
            _c.StoreOffset(msg);

            Log.ForContext("Partition", msg.Partition.Value)
                .ForContext("Topic", msg.TopicPartition.Topic)
                .ForContext("Offset", msg.Offset.Value)
                .Information(
                    "Message of offset: [ {Offset} ], partition: [ {Partition} ], topic: [{Topic}], offset stored");
        }

        public void Seek(TopicPartition topicPartition, long recordOffset)
        {
            var offset = new Offset(recordOffset);
            var tpo = new TopicPartitionOffset(topicPartition, offset);
            _c.Seek(tpo);
        }

        public string GetBootstrapServers()
        {
            return _config.BootstrapServers;
        }

        public string GetKafkaConsumerMaxOffset()
        {
            var sb = new StringBuilder();

            if (_c.Assignment.Count > 0)
            {
                var counter = 0;
                sb.AppendFormat("Part/MaxOffset: ");
                foreach (var item in _c.Assignment)
                {
                    var offset = _c.GetWatermarkOffsets(_c.Assignment[counter]);

                    if (offset.High != -1001)
                    {
                        sb.AppendFormat("[{0}{1}", item.Partition.Value, "/");
                        sb.AppendFormat("{0}]", offset.High);
                        if (counter != _c.Assignment.Count - 1)
                            sb.AppendFormat("{0}", ",");
                    }

                    counter += 1;
                }
            }

            return sb.ToString();
        }

        #endregion Public methods

        #region Private methods

        /// <summary>
        ///     Returns the result of consuming one message from Kafka.
        ///     May throw common Kafka exceptions (as there is no exception handling within this method).
        ///     If successful, will also populate various metadata.
        ///     ==> Will not commit offset! You need to take care of that yourself <==
        /// </summary>
        /// <param name="consumer"></param>
        /// <param name="consumedMessage"></param>
        /// <returns></returns>
        private ConsumeMessageResult ConsumeMessage()
        {
            var returnValue = new ConsumeMessageResult();

            // if we need to hit a certain offset (due to skipping a message for an example), we need to re-seek/re-assign the consumer to that exact offset
            // else the consumer will begin reading from the last commited offset, which may not be what we need it to do...
            //
            // in this case we are looking for a specific offset that we would like to hit -> the value of this offset is stored as _needsToHitOffset
            //
            // this is needed because we might be skipping more than 1 consecutive message, and in this case we must not track
            // the last commited offset nor current consumers in-memory tracked offset => instead we need to make sure we are tracking the _needsToHitOffset
            if (_needsToHitOffset > 0)
            {
                Log.Information("Subscribing to offset: [ " + _needsToHitOffset + " ], partition: [ " +
                                _needsToHitTopicPartition + " ]");

                _c.Subscribe(_topicName);
                _c.Assign(new List<TopicPartitionOffset> { new(_needsToHitTopicPartition, _needsToHitOffset) });

                Log.Warning("Subscribed to offset: [ " + _needsToHitOffset + " ], partition: [ " +
                            _needsToHitTopicPartition + " ]");
            }

            // may throw! caller must handle!
            var consumedMessage = _c.Consume(TimeSpan.FromSeconds(9));

            if (consumedMessage != null && consumedMessage.Message != null)
            {
                _currentOffset = consumedMessage.Offset.Value;
                _currentPartition = consumedMessage.TopicPartition.Partition.Value;
                _consumedMessage = consumedMessage;
                returnValue.CompleteMessage = consumedMessage;
                returnValue.CurrentPartition = consumedMessage.TopicPartition.Partition.Value;
                returnValue.CurrentOffset = consumedMessage.Offset.Value;

                foreach (var headerKey in consumedMessage.Message.Headers)
                {
                }

                // returnValue.CurrentTopicPartitionOffset = consumedMessage.TopicPartitionOffset;
                // if this setting is turned on in consumerconfig
                if (_config.EnablePartitionEof.Value)
                    if (consumedMessage.IsPartitionEOF)
                        return PopulateReturnMessage(returnValue, "EOF");

                if (consumedMessage.Message != null)
                {
                }
            }
            else
            {
                // when topic has no messages (off chance)
                returnValue = PopulateReturnMessage(returnValue, "No new messages");

                if (_needsToHitOffset > 0) returnValue.Offset = _needsToHitOffset;
            }

            return returnValue;
        }

        /// <summary>
        ///     Simple wrap for putting a custom message into return value properties.
        /// </summary>
        /// <param name="thusFar"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private static ConsumeMessageResult PopulateReturnMessage(ConsumeMessageResult thusFar, string message)
        {
            thusFar.Message = "IsPartitionEOF";
            thusFar.ErrorMessage = "IsPartitionEOF";

            return thusFar;
        }

        /// <summary>
        ///     Logs a part of the message that was read from Kafka for logging reasons.
        /// </summary>
        /// <param name="messageConsumingResult"></param>
        private static void ParseAndLog(ConsumeMessageResult messageConsumingResult)
        {
            // here we want to truncate the message so as to avoid flooding the log
            var msgLength = messageConsumingResult.Message?.Length;
            var actualMsg = string.Empty;
            if (msgLength > 40)
                actualMsg = messageConsumingResult.Message.Substring(0, 40);
            else
                actualMsg = messageConsumingResult.Message;

            Log.ForContext("Partition", messageConsumingResult.Partition)
                .ForContext("Topic", messageConsumingResult.Topic)
                .ForContext("Offset", messageConsumingResult.Offset)
                .ForContext("actualMsg", actualMsg)
                .ForContext("GADMMessageId", messageConsumingResult.GadmMessageId)
                .Information(
                    "Message payload: [ {actualMsg} ] <{GADMRequestMethod}> offset: [ {Offset} ] partition: [ {Partition} ]");
        }

        #endregion Private methods
    }
}