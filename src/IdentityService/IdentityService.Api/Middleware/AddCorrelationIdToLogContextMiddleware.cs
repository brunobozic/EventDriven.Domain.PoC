using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace IdentityService.Api.Middleware;

public class AddCorrelationIdToLogContextMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// </summary>
    /// <param name="next"></param>
    public AddCorrelationIdToLogContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task Invoke(HttpContext context)
    {
        LogContext.PushProperty("CorrelationId", context.TraceIdentifier);

        return _next(context);
    }
}