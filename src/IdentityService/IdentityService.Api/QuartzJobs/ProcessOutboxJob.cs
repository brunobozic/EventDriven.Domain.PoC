using IdentityService.Application.CQRSBoilerplate.Command;
using IdentityService.Application.CQRSBoilerplate.OutboxCommands;
using Quartz;
using Serilog;
using System;
using System.Threading.Tasks;

namespace IdentityService.Api.QuartzJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxJob : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await CommandsExecutor.Execute(new ProcessOutboxCommand());
        }
        catch (Exception ex)
        {
            // Log the exception
            Log.Error(ex, "An error occurred while executing ProcessOutboxJob.");
            // Optionally, rethrow or handle accordingly
        }
    }
}