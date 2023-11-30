using Consul;
using EventDriven.Domain.PoC.SharedKernel.HealthChecks.Checks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using HealthStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;

namespace EventDriven.Domain.PoC.Api.Rest.Extensions
{
    public static class ConsulExtensions
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseConsul"))
            {
                services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
                {
                    var address = configuration.GetValue<string>("Consul:Host");
                    if (string.IsNullOrEmpty(address)) throw new ArgumentException(nameof(address));
                    consulConfig.Address = new Uri(address);
                }));
            }

            return services;
        }

        [Obsolete]
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

            if (!(app.Properties["server.Features"] is FeatureCollection features))
                return app;

            var addresses = features.Get<IServerAddressesFeature>();

            //try
            //{   // this will fail if run within the IIS server
            //    var address = addresses.Addresses.First();
            //    Log.Information($"address={address}");
            //}
            //catch (Exception ex)
            //{
            //    Log.Error("Consul fail if run within the IIS server", ex);
            //    throw;
            //}

            // var uri = new Uri(address);
            var registration = new AgentServiceRegistration
            {
                ID = "EventDrivenPoC-5001",
                // service name
                Name = "EventDrivenPoC",
                Address = "localhost",
                Port = 5001,
                Check = new AgentServiceCheck
                {
                    HTTP = "http://localhost:5001/health", // URL of your health check endpoint
                    Interval = TimeSpan.FromSeconds(30), // Interval for health check
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            Log.Information("Registering with Consul");

            try
            {
                consulClient.Agent.ServiceDeregister("EventDrivenPoC-5000").GetAwaiter().GetResult();
                consulClient.Agent.ServiceDeregister(registration.ID).GetAwaiter().GetResult();
            }
            catch (Exception deregistrationException)
            {
            }

            try
            {
                consulClient.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

                lifetime.ApplicationStopping.Register(() =>
                {
                    Log.Information("Unregistering from Consul");
                    consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
                });
            }
            catch (HttpRequestException reqEx)
            {
                Log.Error("Consul not responding, check to see if it is running", reqEx);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error("Consul fail if run within the IIS server", ex);
                throw;
            }

            return app;
        }
    }

    public static class ConsulHealthCheckBuilderExtensions
    {
        private const string NAME = "consul";

        public static IHealthChecksBuilder AddConsul(this IHealthChecksBuilder builder, ConsulOptions setup,
            string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default,
            TimeSpan? timeout = default)
        {
            builder.Services.AddHttpClient();

            var registrationName = name ?? NAME;
            return builder.Add(new HealthCheckRegistration(
                registrationName,
                sp => CreateHealthCheck(sp, setup, registrationName),
                failureStatus,
                tags,
                timeout));
        }

        private static ConsulHealthCheck CreateHealthCheck(IServiceProvider sp, ConsulOptions options, string name)
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            return new ConsulHealthCheck(options, () => httpClientFactory.CreateClient(name));
        }
    }
}