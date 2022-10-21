using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Diagnostics;
using System.IO;

namespace EventDriven.Domain.PoC.Ocelot.ApiGateway
{
    public class Program
    {
#pragma warning disable 1591
        public static readonly string Namespace = typeof(Program).Namespace;
#pragma warning restore 1591
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly string AppName = Namespace;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {
                Log.Warning("Configuring web host ({ApplicationContext})...", AppName);

                var host = CreateWebHostBuilder(args).Build();

                Log.Warning("Starting web host ({ApplicationContext})...", AppName);

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);

                return 1;
            }
        }

#pragma warning disable 1591

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
#pragma warning restore 1591
        {
            return WebHost.CreateDefaultBuilder(args)
                    .CaptureStartupErrors(false)
                    .UseStartup<Startup>()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    //.UseIIS() // <===== For use in "in process" IIS scenarios:
                    .UseSerilog()
                    .UseUrls("http://*:9000")
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config
                            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("ocelot.json", false)
                            .AddEnvironmentVariables()
                            ;
                    })
                //.ConfigureServices(services =>
                //{
                //    services
                //        .AddOcelot()
                //        .AddConsul();
                //})
                //.Configure(app => { app.UseOcelot().Wait(); })
                ;
        }

        [Obsolete]
        private static ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashUrl"];
            var sqlite = configuration["ConnectionStrings:Sqlite"];
            var mssql = configuration["ConnectionStrings:MSSql"];
            var appInstanceName = configuration["InstanceName"];
            var environment = configuration["Environment"];

            // var kafkaProducerForLogging = container.Resolve<IKafkaLoggingProducer>();

            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("Application", appInstanceName)
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .Enrich.WithEnvironmentUserName() // environments are tricky when using a windows service
                                                  //.Enrich.WithExceptionData()
                                                  //.Enrich.WithExceptionStackTraceHash()
                .Enrich.WithMemoryUsage()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.FromLogContext()
                .Enrich.WithProcessName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithEnvironment(environment)
                .Enrich.WithProperty("DebuggerAttached", Debugger.IsAttached)
                .ReadFrom.ConfigurationSection(configuration.GetSection("Serilog"))
                //    .WriteTo.Kafka(kafkaProducerForLogging, new EcsTextFormatter()) // this is how we make the sink use a custom text formatter, in this case, we needed the Elastic compatible formatter
                .WriteTo.Console(theme: AnsiConsoleTheme.Code,
                    outputTemplate:
                    "{Timestamp:HH:mm} [{Level}] [{Address}] {Site}: {Message} || CommandType: [{Command_Type}], CommandId: [{Command_Id}], Application: [{Application}], Machine: [{MachineName}], User: [{EnvironmentUserName}], CorrelationId: [{CorrelationId}], DebuggerAttached: [{DebuggerAttached}] {NewLine}")
                .WriteTo.File(appInstanceName + ".log", rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", // beware this will default to Production appsettings if no ENV is defined on the OS
                    true)
                .AddJsonFile("appsettings.local.json", true,
                    true) //load local settings (usually used for local debugging sessions)
                .AddEnvironmentVariables();

            var config = builder.Build();

            return builder.Build();
        }
    }
}