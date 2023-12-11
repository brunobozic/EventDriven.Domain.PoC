using System;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Framework.Kafka.Core.Contracts;
using IdentityService.Application.CommandsAndHandlers.Users.CUD;
using IdentityService.Application.ViewModels.OutboxMessage;
using MediatR;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers.Database;

namespace IdentityService.Application.CQRSBoilerplate.OutboxCommands;

internal class ProcessOutboxCommandKafkaHandler : ICommandHandler<ProcessOutboxCommand, Unit>
{
    private readonly IKafkaScheduledProducer _kafkaProducer;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public ProcessOutboxCommandKafkaHandler(IKafkaScheduledProducer kafkaProducer,
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _kafkaProducer = kafkaProducer;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();
        const string sql = "SELECT " +
                           "[OutboxMessage].[Id], " +
                           "[OutboxMessage].[Type], " +
                           "[OutboxMessage].[Data] " +
                           "FROM [OutboxMessages] AS [OutboxMessage] " +
                           "WHERE [OutboxMessage].[ProcessedDate] IS NULL";

        var messages = await connection.QueryAsync<OutboxMessageDto>(sql);
        var messagesList = messages.AsList();

        // "UPDATE [dbo].[OutboxMessages] " + "SET [ProcessedDate] = @Date " + "WHERE [Id] = @Id"; // <=== notice the schema prefix, this might be needed for certain db engines
        const string sqlUpdateProcessedDate = "UPDATE [OutboxMessages] " +
                                              "SET [ProcessedDate] = @Date " +
                                              "WHERE [Id] = @Id";
        if (messagesList.Count > 0)
        {
            var activitySource = new ActivitySource("OtPrGrJa");
            foreach (var message in messagesList)
            {
                using var activity = activitySource.StartActivity("ProcessOutboxMessage");
                activity?.SetTag("Message.Type", message.Type);
                activity?.SetTag("Message.Id", message.Id);

                using (LogContext.PushProperty("MessageType", message.Type))
                using (LogContext.PushProperty("MessageId", message.Id))
                {
                    try
                    {
                        var type = typeof(CreateUserCommandHandler).Assembly.GetType(message.Type);

                        activity?.SetTag("Type of message put on Outbox", message.Type);
                        activity?.SetTag("Message.Id", message.Id);

                        var integrationEvent =
                            JsonConvert.DeserializeObject(message.Data, type) as IIntegrationEventNotification;

                        using (LogContext.Push(new OutboxMessageContextEnricher(integrationEvent)))
                        {
                            #region Publish the integration event to Kafka

                            var success = await _kafkaProducer.WriteMessageAsync(message.Data);

                            #endregion Publish the integration event to Kafka

                            if (success)
                            {
                                // be mindfull that in certain cases the raw sql for this might have to include the schema name (eg. [dbo].[OutboxMessages])
                                // if you encounter problems similar to "no table found", try adding the schema name to the table name
                                var storeResult = await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                                {
                                    Date = DateTime.UtcNow,
                                    message.Id
                                });

                                activity?.SetTag("IntegrationEvent Id", $"OutboxMessage:{integrationEvent.Id}");
                                activity?.SetTag("Message stored into Kafka", storeResult);
                                var ae = new ActivityEvent("Message stored into Kafka");
                                activity?.AddEvent(ae);
                            }
                            else
                            {
                                var ae = new ActivityEvent("Message not stored into Kafka");
                                activity?.AddEvent(ae);
                                activity?.SetStatus(Status.Error);
                            }
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        Log.Error(jsonEx, "JSON deserialization failed for message {MessageId}", message.Id);
                        activity?.RecordException(jsonEx);
                        activity?.SetStatus(ActivityStatusCode.Error);
                        throw;
                    }
                    catch (DbException dbEx)
                    {
                        Log.Error(dbEx, "Database operation failed for message {MessageId}", message.Id);
                        activity?.RecordException(dbEx);
                        activity?.SetStatus(ActivityStatusCode.Error);
                        throw;
                    }
                    catch (Exception possibleAsyncProblems)
                    {
                        Log.Error(possibleAsyncProblems.Message, possibleAsyncProblems);
                        activity?.RecordException(possibleAsyncProblems);
                        activity?.SetStatus(Status.Error);
                        // this will break the consistency of the app, better stop and investigate
                        // possible outcomes might include perpetual sending of the same set of messages to kafka
                        // obviously kafka would survice but the messages would then need to be cleaned up, which might end up in a maintenance nightmare
                        throw;
                    }
                }
            }
        }

        return Unit.Value;
    }

    private class OutboxMessageContextEnricher : ILogEventEnricher
    {
        private readonly IIntegrationEventNotification _integrationEvent;

        public OutboxMessageContextEnricher(IIntegrationEventNotification integrationEvent)
        {
            _integrationEvent = integrationEvent;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(new LogEventProperty("Context",
                new ScalarValue($"OutboxMessage:{_integrationEvent.Id}")));
        }
    }
}