using IdentityService.Application.CQRSBoilerplate;
using IdentityService.Application.CQRSBoilerplate.Command;
using Quartz;
using System.Threading.Tasks;

namespace IdentityService.Api.QuartzJobs;

[DisallowConcurrentExecution]
public class ProcessInboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await CommandsExecutor.Execute(new ProcessInboxCommand());
    }
}

