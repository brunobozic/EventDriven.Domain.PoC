namespace EventDriven.Domain.PoC.Api.Rest.Middleware
{
    //public class AdUserMiddleware
    //{
    //    private readonly RequestDelegate next;

    //    public AdUserMiddleware(RequestDelegate next)
    //    {
    //        this.next = next;
    //    }

    //    public async Task Invoke(HttpContext context, IUserProvider userProvider, IConfiguration config, IWebHostEnvironment environment)
    //    {
    //        var useAd = true;
    //        bool? configValue = null;

    //        try
    //        {
    //            configValue = config.GetSection("MyConfigurationValues").GetValue<bool>("UseActiveDirectory");
    //        }
    //        catch (Exception ex)
    //        {
    //            configValue = null;
    //        }

    //        if (configValue.HasValue) useAd = configValue.Value;

    //        if (useAd)
    //            if (!(userProvider.Initialized))
    //            {
    //                await userProvider.Create(context, config, environment);
    //            }

    //        await next(context);
    //    }
    //}
}