using EventDriven.Domain.PoC.SharedKernel.HealthChecks.Checks;
using Microsoft.Extensions.DependencyInjection;

namespace EventDriven.Domain.PoC.SharedKernel.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddFileWritePermissionsCheck(this IHealthChecksBuilder builder,
            string folderToTest)
        {
            var check = new VerifyWritePermissionsHealthCheck(folderToTest);
            builder.AddCheck("Check folder write permissions", check);

            return builder;
        }
    }
}