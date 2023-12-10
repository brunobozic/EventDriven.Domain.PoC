using System.Threading.Tasks;
using Quartz;

namespace IdentityService.Api.QuartzJobs;

[DisallowConcurrentExecution]
public class KafkaPollJob : IJob
{
    private readonly IJobController _jc;

    public KafkaPollJob(IJobController jc)
    {
        _jc = jc;
    }

    public Task Execute(IJobExecutionContext context)
    {
        _jc.ReadAndProcessKafkaMessage(context.JobDetail.JobDataMap);

        return Task.CompletedTask;

        //// needed to implement a feature that would stop reading of messages after having read a single production kafka message
        //return Task.FromException(new System.Exception("Force stop for testing purposes."));
    }
}