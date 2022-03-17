using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Api.Rest.Middleware
{
    public class SerilogMiddleware
    {
        private readonly ILogger<SerilogMiddleware> _logger;
        private readonly RequestDelegate next;
        private Func<string, Exception, string> _defaultFormatter = (state, exception) => state;

        public SerilogMiddleware(RequestDelegate next, ILogger<SerilogMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var url = context.Request.GetDisplayUrl();

            // Log.Warning($"\n{DateTime.Now} => Request url: {url}, \n Request Method: {context.Request.Method}, Request Scheme: {context.Request.Scheme}, UserAgent: {context.Request.Headers[HeaderNames.UserAgent]}");

            var requestUrlString =
                $"Request url: {url} \n Request Method: {context.Request.Method}, Request Schema: {context.Request.Scheme}, UserAgent: {context.Request.Headers[HeaderNames.UserAgent]}";

            var allkeypair = "";
            var headers = context.Request.Headers;

            foreach (var headerValuePair in headers)
                allkeypair += "\n" + headerValuePair.Key + ":" + headerValuePair.Value;

            var requestHeadersString = $"     => Request.Headers:\n {allkeypair}";

            // Log.Warning($"\n{DateTime.Now} => Request.Headers:{0}\n" + allkeypair.ToString());

            var requestBodyStream = new MemoryStream();
            var originalRequestBody = context.Request.Body;

            await context.Request.Body.CopyToAsync(requestBodyStream);
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
            var requestBodyString = "";

            if (!string.IsNullOrEmpty(requestBodyText))
                // Log.Warning($"\n{DateTime.Now} => Request Body: {requestBodyText}");

                requestBodyString = $"     => Request Body: {requestBodyText}";

            requestBodyStream.Seek(0, SeekOrigin.Begin);

            context.Request.Body = requestBodyStream;

            if (!string.IsNullOrEmpty(requestBodyString))
                Log.Warning("{0} => {1}\n{2}\n{3}\n", DateTime.Now.ToLocalTime(), requestUrlString,
                    requestHeadersString, requestBodyString);
            else
                Log.Warning("{0} => {1}\n{2}\n", DateTime.Now.ToLocalTime(), requestUrlString, requestHeadersString);

            await next(context);

            context.Request.Body = originalRequestBody;
        }

        private static async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{responseBody}";
        }
    }
}