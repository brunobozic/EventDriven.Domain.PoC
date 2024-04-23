using IdentityService.Application.CQRSBoilerplate.Command;
using IdentityService.Application.CQRSBoilerplate.OutboxCommands;
using Quartz;
using System.Threading.Tasks;

namespace IdentityService.Api.QuartzJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ProcessOutboxCommand());
    }
}