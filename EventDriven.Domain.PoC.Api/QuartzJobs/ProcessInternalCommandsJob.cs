using EventDriven.Domain.PoC.Application.CQRSBoilerplate.Command;
using EventDriven.Domain.PoC.Application.CQRSBoilerplate.InternalCommands;
using Quartz;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Api.Rest.QuartzJobs
{
    [DisallowConcurrentExecution]
    public class ProcessInternalCommandsJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.Execute(new ProcessInternalCommandsCommand());
        }
    }
}