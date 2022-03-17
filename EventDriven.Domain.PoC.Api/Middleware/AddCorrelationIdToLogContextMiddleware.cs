using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Api.Rest.Middleware
{
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
}