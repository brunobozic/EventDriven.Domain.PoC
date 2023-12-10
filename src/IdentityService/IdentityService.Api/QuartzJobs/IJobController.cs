using Quartz;

namespace IdentityService.Api.QuartzJobs;

public interface IJobController
{
    void ReadAndProcessKafkaMessage(JobDataMap jobDataMap);
}