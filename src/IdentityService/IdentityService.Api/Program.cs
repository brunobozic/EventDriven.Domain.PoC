using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace IdentityService.Api;
public class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateWebHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(
                webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}