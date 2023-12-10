using Microsoft.Extensions.DependencyInjection;
using SharedKernel.HealthChecks.Checks;

namespace SharedKernel.HealthChecks;

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