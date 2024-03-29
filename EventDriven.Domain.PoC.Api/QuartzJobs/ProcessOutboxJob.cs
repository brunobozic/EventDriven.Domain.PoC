﻿using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.CQRSBoilerplate.Command;
using EventDriven.Domain.PoC.Application.CQRSBoilerplate.OutboxCommands;
using Quartz;

namespace EventDriven.Domain.PoC.Api.Rest.QuartzJobs
{
    [DisallowConcurrentExecution]
    public class ProcessOutboxJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await CommandsExecutor.Execute(new ProcessOutboxCommand());
        }
    }
}