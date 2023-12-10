using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers.Database;

namespace IdentityService.Application.CQRSBoilerplate.Command;

public class CommandsScheduler : ICommandsScheduler
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        var activitySource = new ActivitySource("OtPrGrJa");
        using var activity = activitySource.StartActivity("CommandsScheduler");

        using var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sqlInsert =
            "INSERT INTO [InternalCommands] ([Id], [EnqueueDate], [Type], [Data]) VALUES " +
            "(@Id, @EnqueueDate, @Type, @Data)";

        try
        {
            await connection.ExecuteAsync(sqlInsert, new
            {
                command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = command.GetType().FullName,
                Data = JsonConvert.SerializeObject(command)
            });

            activity?.SetTag("CommandId", command.Id.ToString());
            activity?.SetTag("CommandType", command.GetType().FullName);
        }
        catch (Exception ex)
        {
            activity?.RecordException(ex);
            activity?.SetStatus(ActivityStatusCode.Error);
            throw;
        }
    }
}