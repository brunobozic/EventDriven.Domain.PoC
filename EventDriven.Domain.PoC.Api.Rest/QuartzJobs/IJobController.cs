using Quartz;

namespace EventDriven.Domain.PoC.Api.Rest.QuartzJobs
{
    public interface IJobController
    {
        void ReadAndProcessKafkaMessage(JobDataMap jobDataMap);
    }
}