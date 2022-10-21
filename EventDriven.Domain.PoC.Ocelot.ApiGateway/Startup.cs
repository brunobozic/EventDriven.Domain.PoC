using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics;
using ILogger = Serilog.ILogger;

namespace EventDriven.Domain.PoC.Ocelot.ApiGateway
{
    public class Startup
    {
        public static readonly string Namespace = typeof(Program).Namespace;

        public static readonly string AppName = Namespace;

        public Startup(
            IConfiguration configuration
            , ILoggerFactory loggerFactory
            , IWebHostEnvironment env
        )
        {
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            Env = env;
            Logger = ConfigureLogger(configuration);
            LoggerFactory.AddSerilog(Logger);
        }

        public IWebHostEnvironment Env { get; set; }

        public ILogger Logger { get; }

        public IConfiguration Configuration { get; }

        public ILoggerFactory LoggerFactory { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1",
            //        new OpenApiInfo {Title = "EventDriven.Domain.PoC.Ocelot.ApiGateway", Version = "v1"});
            //});

            services.AddOcelot().AddConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EventDriven.Domain.PoC.Ocelot.ApiGateway v1"));

            // app.UseHttpsRedirection();

            // app.UseRouting();

            // app.UseAuthorization();

            app.UseOcelot().Wait();

            // app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private ILogger ConfigureLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashUrl"];
            var sqlite = configuration["ConnectionStrings:Sqlite"];
            var mssql = configuration["ConnectionStrings:MSSql"];
            var appInstanceName = configuration["InstanceName"];
            var environment = configuration["Environment"];

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
                .WriteTo.Console(theme: AnsiConsoleTheme.Code,
                    outputTemplate:
                    "{Timestamp:HH:mm} [{Level}] [{Address}] {Site}: {Message} || Application: [{Application}], Machine: [{MachineName}], User: [{EnvironmentUserName}], CorrelationId: [{CorrelationId}], DebuggerAttached: [{DebuggerAttached}] {NewLine}")
                .WriteTo.File(appInstanceName + ".log", rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null)
                .CreateLogger();
        }
    }
}