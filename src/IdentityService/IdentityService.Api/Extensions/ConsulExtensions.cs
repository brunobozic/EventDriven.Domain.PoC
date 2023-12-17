using System;
using System.Collections.Generic;
using System.Net.Http;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SharedKernel.HealthChecks.Checks;
using HealthStatus = Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus;
using ILogger = Serilog.ILogger;

namespace IdentityService.Api.Extensions;

public static class ConsulExtensions
{
    public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseConsul"))
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration.GetValue<string>("Consul:Host");
                if (string.IsNullOrEmpty(address)) throw new ArgumentException(nameof(address));
                consulConfig.Address = new Uri(address);
            }));

        return services;
    }

    public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
    {
        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("ConsulExtensions");

        var serviceId = configuration.GetValue<string>("Consul:ServiceId") ?? "EventDrivenPoC";
        var serviceName = configuration.GetValue<string>("Consul:ServiceName") ?? "EventDrivenPoC";
        var serviceAddress = configuration.GetValue<string>("Consul:ServiceAddress") ?? "localhost";
        var servicePort = configuration.GetValue<int>("Consul:ServicePort");

        var registration = new AgentServiceRegistration
        {
            ID = serviceId,
            Name = serviceName,
            Address = serviceAddress,
            Port = servicePort,
            Check = new AgentServiceCheck
            {
                HTTP = $"http://{serviceAddress}:{servicePort}/health",
                Interval = TimeSpan.FromSeconds(30),
                Timeout = TimeSpan.FromSeconds(5)
            }
        };

        Log.Information("Registering with Consul");
        RegisterServiceWithConsul(consulClient, registration, lifetime);

        return app;
    }

    private static void RegisterServiceWithConsul(IConsulClient consulClient, AgentServiceRegistration registration, IHostApplicationLifetime lifetime)
    {
        try
        {
            consulClient.Agent.ServiceDeregister(registration.ID).GetAwaiter().GetResult();
            consulClient.Agent.ServiceRegister(registration).GetAwaiter().GetResult();

            lifetime.ApplicationStopping.Register(() =>
            {
                Log.Information("Deregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during Consul registration or deregistration.");
            throw;
        }
    }
}
