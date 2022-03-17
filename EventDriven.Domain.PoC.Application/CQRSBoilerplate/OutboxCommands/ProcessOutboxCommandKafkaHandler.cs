using Dapper;
using EventDriven.Domain.PoC.Application.ViewModels.OutboxMessage;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Database;
using Framework.Kafka.Core.Contracts;
using MediatR;
using Newtonsoft.Json;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.OutboxCommands
{
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

            const string sqlUpdateProcessedDate = "UPDATE [app].[OutboxMessages] " +
                                                  "SET [ProcessedDate] = @Date " +
                                                  "WHERE [Id] = @Id";
            if (messagesList.Count > 0)
                foreach (var message in messagesList)
                {
                    var type = typeof(User).Assembly.GetType(message.Type);

                    var integrationEvent =
                        JsonConvert.DeserializeObject(message.Data, type) as IIntegrationEventNotification;

                    using (LogContext.Push(new OutboxMessageContextEnricher(integrationEvent)))
                    {
                        #region Publish the integration event to Kafka

                        await _kafkaProducer.WriteMessageAsync(message.Data);

                        #endregion Publish the integration event to Kafka

                        await connection.ExecuteAsync(sqlUpdateProcessedDate, new
                        {
                            Date = DateTime.UtcNow,
                            message.Id
                        });
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
}