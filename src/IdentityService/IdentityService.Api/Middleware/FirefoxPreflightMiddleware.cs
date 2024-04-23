using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IdentityService.Api.Middleware;

public class FirefoxPreflightMiddleware
{
    private readonly RequestDelegate _next;

    public FirefoxPreflightMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        if (context.Request.Method == "OPTIONS")
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin",
                new[] { (string)context.Request.Headers["Origin"] });
            context.Response.Headers.Add("Access-Control-Allow-Headers",
                new[] { "Origin, X-Requested-With, Content-Type, Accept, Authentication" });
            context.Response.Headers.Add("Access-Control-Allow-Methods", new[] { "GET, POST, PUT, DELETE, OPTIONS" });
            context.Response.Headers.Add("Access-Control-Allow-Credentials", new[] { "true" });
            context.Response.StatusCode = 200;
            return context.Response.WriteAsync("OK");
        }

        return _next(context);
    }
}