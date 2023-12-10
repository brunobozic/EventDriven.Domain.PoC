using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Api.Middleware;

public class AddCorrelationIdToResponseMiddleware
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    /// <summary>
    /// </summary>
    /// <param name="next"></param>
    public AddCorrelationIdToResponseMiddleware(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task Invoke(HttpContext context)
    {
        //context
        //    .Response
        //    .Headers
        //    .Add(CorrelationIdHeaderName, context.TraceIdentifier);

        context.Response.OnStarting(() =>
        {
            var oldHeaders = context.Request.Headers;
            if (!context.Response.Headers.ContainsKey(CorrelationIdHeaderName))
                context.Response.Headers.Add(CorrelationIdHeaderName, new[] { context.TraceIdentifier });
            return Task.CompletedTask;
        });

        await _next(context);
    }
}