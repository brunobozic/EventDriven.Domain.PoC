using System.Threading.Tasks;

namespace SharedKernel.HealthChecks.Base;

/// <summary>
///     Defines the interface for a health check provider.
/// </summary>
public interface IHealthCheckProvider
{
    /// <summary>
    ///     Defines the order of this provider in the results.
    /// </summary>
    int SortOrder { get; }

    /// <summary>
    ///     Returns the health heck info.
    /// </summary>
    Task<HealthCheckItemResult> GetHealthCheckAsync();
}