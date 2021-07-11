using System;
using System.Threading.Tasks;
using Dapper;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Database;
using Newtonsoft.Json;

namespace EventDriven.Domain.PoC.Application.CQRSBoilerplate.Command
{
    public class CommandsScheduler : ICommandsScheduler
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task EnqueueAsync<T>(ICommand<T> command)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            const string sqlInsert =
                "INSERT INTO [InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
                "(@Id, @EnqueueDate, @Type, @Data)";

            await connection.ExecuteAsync(sqlInsert, new
            {
                command.Id,
                EnqueueDate = DateTime.UtcNow,
                Type = command.GetType().FullName,
                Data = JsonConvert.SerializeObject(command)
            });
        }
    }
}