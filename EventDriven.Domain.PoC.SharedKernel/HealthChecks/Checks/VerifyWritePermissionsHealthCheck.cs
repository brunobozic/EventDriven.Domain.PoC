using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.SharedKernel.HealthChecks.Checks
{
    public class VerifyWritePermissionsHealthCheck : IHealthCheck
    {
        private readonly string _folder;

        public VerifyWritePermissionsHealthCheck(string folder)
        {
            _folder = folder;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new())
        {
            try
            {
                var fileName = Path.Combine(_folder, $"{Guid.NewGuid()}.txt");
                File.WriteAllText(fileName, "Test from health check");
                File.Delete(fileName);
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch
            {
                return Task.FromResult(HealthCheckResult.Unhealthy());
            }
        }
    }
}