using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Repository.EF.DatabaseContext
{
    public class MyContextContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (string.IsNullOrEmpty(envName))
            {
#if DEBUG
                envName = "Development";
#endif
#if !DEBUG
                envName = "Production";
#endif
            }

            Log.Warning("MyContextContextFactory Environment: " + envName);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                    false) // beware this will default to Production appsettings if no ENV is defined on the OS                                                                                                                            // .AddJsonFile("appsettings.local.json", true) // load local settings (usually used for local debugging sessions)  ==> this will override all the other previously loaded appsettings, so comment this out in production!
                           //.AddJsonFile("appsettings.local.json", true)
                .Build();

            // Here we create the DbContextOptionsBuilder manually.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Build connection string. This requires that you have a connectionstring in the appsettings.json
            //var connectionString = configuration.GetConnectionString("MSSql");
            var connectionString = configuration.GetConnectionString("Sqlite");

            Log.Warning("MyContextContextFactory GetConnectionString: " + connectionString);

            builder.UseSqlite(connectionString);
            // builder.UseNpgsql(connectionString);

            // Create our DbContext.
            return new ApplicationDbContext(builder.Options /*, new NoMediator()*/, true);
        }

        private string GetBasePath()
        {
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }

        public class NoMediator : IMediator
        {
            public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public IAsyncEnumerable<object> CreateStream(object request, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task Publish<TNotification>(TNotification notification,
                CancellationToken cancellationToken = default) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
                CancellationToken cancellationToken = default)
            {
                return Task.FromResult(default(TResponse));
            }

            public Task<object> Send(object request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(default(object));
            }

            public Task Send(IRequest request, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }
    }
}