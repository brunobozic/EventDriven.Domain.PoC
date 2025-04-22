using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContexts;
using IdentityService.Data.Seed;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Diagnostics;
using System.IO;

namespace IdentityService.Api;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

public class Program
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
{
#pragma warning disable 1591
    public static readonly string Namespace = typeof(Program).Namespace;
#pragma warning restore 1591
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static readonly string AppName = Namespace;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    [Obsolete]
    public static int Main(string[] args)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        var configuration = GetConfiguration();

        Log.Logger = CreateSerilogLogger(configuration);

        try
        {
            Log.Warning("Configuring web host ({ApplicationContext})...", AppName);

            var host = BuildWebHost(configuration, args);

            using (var scope = host.Services.CreateScope())
            {
                var environment = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
                if (environment.IsDevelopment()) // Seeding only in Development
                {
                    var databaseInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
                    databaseInitializer.MigrateAndSeed();
                }
            }

            Log.Warning("Starting web host ({ApplicationContext})...", AppName);

            host.Run();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);

            Debug.WriteLine(ex.Message);

            Console.WriteLine(ex.Message);

            return 1;
        }
    }

#pragma warning disable 1591

    public static IWebHost BuildWebHost(IConfiguration configuration, string[] args)
#pragma warning restore 1591
    {
        return WebHost.CreateDefaultBuilder(args)
            .CaptureStartupErrors(true)
            .UseStartup<Startup>()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseConfiguration(configuration)
            //.UseIIS() // <===== For use in "in process" IIS scenarios:
            //.UseKestrel(opts =>
            //{
            //    // Bind directly to a socket handle or Unix socket
            //    // opts.ListenHandle(123554);
            //    // opts.ListenUnixSocket("/tmp/kestrel-test.sock");
            //    opts.Listen(IPAddress.Loopback, port: 6000);
            //    // opts.ListenAnyIP(80);
            //    opts.ListenLocalhost(6000);
            //    //opts.ListenLocalhost(6001, opts => opts.UseHttps());
            //    //opts.ListenLocalhost(6000);
            //})
            ////.UseUrls("http://+:6000"/*, "https://+:6001"*/)
            //.UseUrls("http://localhost:6005", "https://+:6006")
            //.UseSerilog()
            .Build();
    }

    [Obsolete]
    private static ILogger CreateSerilogLogger(IConfiguration configuration)
    {
        var appInstanceName = configuration["InstanceName"];
        var environment = configuration["Environment"];
        var elasticsearchUrl = configuration["MyConfigurationValues:ElasticSearchUrl"]; // Ensure you have this in your appsettings.json

        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty("Application", appInstanceName)
            .Enrich.WithProperty("Environment", environment)
            .Enrich.WithAssemblyName()
            .Enrich.WithAssemblyVersion()
            .Enrich.WithEnvironmentUserName() // environments are tricky when using a windows service
            .Enrich.WithExceptionData()
            //.Enrich.WithExceptionStackTraceHash()
            .Enrich.WithMemoryUsage()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName()
            .Enrich.FromLogContext()
            .Enrich.WithProcessName()
            .Enrich.WithEnvironmentUserName()
            .Enrich.WithEnvironment(environment)
            .Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached)
            .WriteTo.Console(theme: AnsiConsoleTheme.Code,
                outputTemplate:
                "{Timestamp:HH:mm} [{Level}] [{Address}] {Site}: {Message} || CommandType: [{Command_Type}], CommandId: [{Command_Id}], Application: [{Application}], Machine: [{MachineName}], User: [{EnvironmentUserName}], CorrelationId: [{CorrelationId}], DebuggerAttached: [{DebuggerAttached}] {NewLine}")
            .WriteTo.File(appInstanceName + ".log", rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: null)
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
               {
                   AutoRegisterTemplate = true,
                   AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7, // Make sure this matches your Elasticsearch version
                   IndexFormat = $"{appInstanceName.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
               })
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .CreateLogger();
    }

    private static IConfiguration GetConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", // beware this will default to Production appsettings if no ENV is defined on the OS
                true)
            .AddEnvironmentVariables();

        var config = builder.Build();

        return builder.Build();
    }
}