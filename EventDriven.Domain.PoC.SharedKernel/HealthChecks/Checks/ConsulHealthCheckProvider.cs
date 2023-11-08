using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.SharedKernel.HealthChecks.Checks
{
    public class ConsulHealthCheck : IHealthCheck
    {
        private readonly Func<HttpClient> _httpClientFactory;
        private readonly ConsulOptions _options;

        public ConsulHealthCheck(ConsulOptions options, Func<HttpClient> httpClientFactory)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory();
                if (_options.RequireBasicAuthentication)
                {
                    var credentials = Encoding.ASCII.GetBytes($"{_options.Username}:{_options.Password}");
                    var authHeaderValue = Convert.ToBase64String(credentials);

                    client.DefaultRequestHeaders
                        .Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
                }

                var result = await client
                    .GetAsync($"{(_options.RequireHttps ? "https" : "http")}://{_options.HostName}:{_options.Port}/hc",
                        cancellationToken);

                return result.IsSuccessStatusCode
                    ? HealthCheckResult.Healthy()
                    : new HealthCheckResult(context.Registration.FailureStatus,
                        "Consul response was not a successful HTTP status code");
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }

    public class ConsulOptions
    {
        public string HostName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool RequireBasicAuthentication { get; set; }
        public bool RequireHttps { get; set; }
        public string Username { get; set; }
    }
}