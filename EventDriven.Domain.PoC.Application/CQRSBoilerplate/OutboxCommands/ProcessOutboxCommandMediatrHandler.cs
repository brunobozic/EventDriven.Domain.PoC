using Dapper;
using EventDriven.Domain.PoC.Application.EventsAndEventHandlers.Users.CUD.Notifications;
using EventDriven.Domain.PoC.Application.ViewModels.OutboxMessage;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Database;
using MediatR;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.OutboxCommands
{
    //internal class ProcessOutboxCommandMediatrHandler : ICommandHandler<ProcessOutboxCommand, Unit>
    //{
    //    private readonly IMediator _mediator;

    //    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    //    public ProcessOutboxCommandMediatrHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
    //    {
    //        _mediator = mediator;
    //        _sqlConnectionFactory = sqlConnectionFactory;
    //    }

    //    public async Task<Unit> Handle(ProcessOutboxCommand command, CancellationToken cancellationToken)
    //    {
    //        var connection = _sqlConnectionFactory.GetOpenConnection();
    //        const string sql = "SELECT " +
    //                           "[OutboxMessage].[Id], " +
    //                           "[OutboxMessage].[Type], " +
    //                           "[OutboxMessage].[Data] " +
    //                           "FROM [OutboxMessages] AS [OutboxMessage] " +
    //                           "WHERE [OutboxMessage].[ProcessedDate] IS NULL";

    //        try
    //        {
    //            var messages = await connection.QueryAsync<OutboxMessageDto>(sql);
    //            var messagesList = messages.AsList();

    //            const string sqlUpdateProcessedDate = "UPDATE [OutboxMessages] " +
    //                                                  "SET [ProcessedDate] = @Date " +
    //                                                  "WHERE [Id] = @Id";
    //            if (messagesList.Count > 0)
    //                foreach (var message in messagesList)
    //                {
    //                    var type = typeof(UserCreatedNotification).Assembly.GetType(message.Type);

    //                    var request =
    //                        JsonConvert.DeserializeObject(message.Data, type) as IIntegrationEventNotification;

    //                    using (LogContext.Push(new OutboxMessageContextEnricher(request)))
    //                    {
    //                        #region Publish the integration event to mediatR

    //                        await _mediator.Publish(request, cancellationToken);

    //                        #endregion Publish the integration event to mediatR

    //                        await connection.ExecuteAsync(sqlUpdateProcessedDate, new
    //                        {
    //                            Date = DateTime.UtcNow,
    //                            message.Id
    //                        });
    //                    }
    //                }
    //        }
    //        catch (Exception e)
    //        {
    //            Log.Fatal(e.Message, e);
    //            throw;
    //        }


    //        return Unit.Value;
    //    }

    //    private class OutboxMessageContextEnricher : ILogEventEnricher
    //    {
    //        private readonly IIntegrationEventNotification _notification;

    //        public OutboxMessageContextEnricher(IIntegrationEventNotification notification)
    //        {
    //            _notification = notification;
    //        }

    //        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    //        {
    //            logEvent.AddOrUpdateProperty(new LogEventProperty("Context",
    //                new ScalarValue($"OutboxMessage:{_notification.Id}")));
    //        }
    //    }
    //}
}