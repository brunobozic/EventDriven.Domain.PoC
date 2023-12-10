using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using IdentityService.Application.CQRSBoilerplate.Command;
using MediatR;
using Newtonsoft.Json;
using OpenTelemetry.Trace;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers.Database;

namespace IdentityService.Application.CQRSBoilerplate.InternalCommands;

internal class ProcessInternalCommandsCommandHandler : ICommandHandler<ProcessInternalCommandsCommand, Unit>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public ProcessInternalCommandsCommandHandler(
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Unit> Handle(ProcessInternalCommandsCommand command, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sql = @"SELECT [Type], [Data] 
                         FROM [InternalCommands] 
                         WHERE [ProcessedDate] IS NULL";
        var internalCommandsList = (await connection.QueryAsync<InternalCommandDto>(sql)).AsList();

        foreach (var internalCommand in internalCommandsList)
        {
            var activitySource = new ActivitySource("OtPrGrJa");
            using var activity = activitySource.StartActivity("ProcessInternalCommandsCommandHandler");

            var type = Type.GetType(internalCommand.Type);

            try
            {
                dynamic commandToProcess = JsonConvert.DeserializeObject(internalCommand.Data, type);

                activity?.SetTag("CommandType", type.FullName);
                activity?.SetTag("CommandData", internalCommand.Data);

                await CommandsExecutor.Execute(commandToProcess);
            }
            catch (Exception ex)
            {
                activity?.RecordException(ex);
                activity?.SetStatus(ActivityStatusCode.Error);
                // Consider how to handle the exception. Log or rethrow?
            }
        }

        return Unit.Value;
    }


    private class InternalCommandDto
    {
        public string Data { get; set; }
        public string Type { get; set; }
    }
}