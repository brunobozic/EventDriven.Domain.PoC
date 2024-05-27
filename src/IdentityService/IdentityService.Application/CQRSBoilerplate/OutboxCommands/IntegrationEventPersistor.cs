using Autofac;
using Dapper;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers.Database;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace IdentityService.Application.CQRSBoilerplate.OutboxCommands;

public class IntegrationEventPersistor : IIntegrationEventPersistor
{
    public async Task InsertIntoInbox(IIntegrationEvent<IDomainEvent> @event)
    {
        var activitySource = new ActivitySource("OtPrGrJa");
        using var activity = activitySource.StartActivity("IntegrationEventPersistor");
        using var scope = CompositionRoot.BeginLifetimeScope();
        using var connection = scope.Resolve<ISqlConnectionFactory>().GetOpenConnection();

        string type = @event.GetType().FullName;

        var data = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
        {
            ContractResolver = new AllPropertiesContractResolver()
        });

        var sql = "INSERT INTO [InboxMessages] (Id, OccurredOn, Type, Data) " +
                  "VALUES (@Id, @OccurredOn, @Type, @Data)";
        try
        {
            await connection.ExecuteScalarAsync(sql, new
            {
                @event.Id,
                OccurredOn = DateTime.UtcNow,
                Type = @event.GetType().FullName,
                data
            });

            activity?.SetTag("EventId", @event.Id.ToString());
            activity?.SetTag("EventType", @event.GetType().FullName);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            activity?.SetStatus(ActivityStatusCode.Error);

            throw;
        }
    }
}

