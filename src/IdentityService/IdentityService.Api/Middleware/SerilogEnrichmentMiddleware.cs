using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

public class SerilogEnrichmentMiddleware
{
    private readonly RequestDelegate _next;

    public SerilogEnrichmentMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Use the TraceIdentifier as a correlation ID.
        var traceIdentifier = context.TraceIdentifier;

        // Enrich the Serilog log context with the TraceIdentifier
        using (LogContext.PushProperty("CorrelationId", traceIdentifier))
        {
            await _next(context);
        }
    }
}