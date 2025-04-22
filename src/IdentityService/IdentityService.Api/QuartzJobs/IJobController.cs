using Quartz;
using System.Threading.Tasks;

namespace IdentityService.Api.QuartzJobs;

public interface IJobController
{
    Task ReadAndProcessKafkaMessageAsync(JobDataMap jobDataMap);
}