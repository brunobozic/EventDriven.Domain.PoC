using IdentityService.Api;
using IdentityService.Data.DatabaseContexts;
using IdentityService.Data.DatabaseContexts.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using URF.Core.Abstractions;
using URF.Core.EF;

namespace IdentityService.API.REST.Test.Integration;

public class TestBase
{
    public CustomWebApplicationFactory<Program> Factory { get; set; }
    public HttpClient Client { get; set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        Factory = new CustomWebApplicationFactory<Program>()
            //.WithWebHostBuilder(builder =>
            //{
            //    //builder.ConfigureServices(ConfigureServices)
            //    // .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            //    // .ConfigureContainer<ContainerBuilder>(ConfigureAutofac);
            //})
            ;

        Client = Factory.CreateClient();
    }


    protected virtual void ConfigureServices(IServiceCollection services)
    {
        //var descriptor = services.SingleOrDefault(
        //    d => d.ServiceType == typeof(DbContextOptions<DistDigitalizationDbContext>));
        //if (descriptor != null)
        //{
        //    services.Remove(descriptor);
        //}

        //services.AddDbContext<DistDigitalizationDbContext>(options => options.UseInMemoryDatabase("SafeRemoveTestDb"));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase("InMemoryDbForTesting"));
        services.AddTransient<ApplicationDbContext, ApplicationDbContext>();
        services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        // services.AddTransient<IEntityBase, BaseEntity>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Client.Dispose();
        Factory.Dispose();
    }
}
