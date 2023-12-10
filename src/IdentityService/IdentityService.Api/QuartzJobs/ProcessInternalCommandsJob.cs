using System.Threading.Tasks;
using IdentityService.Application.CQRSBoilerplate.Command;
using IdentityService.Application.CQRSBoilerplate.InternalCommands;
using Quartz;

namespace IdentityService.Api.QuartzJobs;

[DisallowConcurrentExecution]
public class ProcessInternalCommandsJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ProcessInternalCommandsCommand());
    }
}