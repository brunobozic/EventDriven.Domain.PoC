using EventDriven.Domain.PoC.Domain.DomainEntities.DomainExceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System;
using System.Net;

namespace EventDriven.Domain.PoC.Api.Rest.Helpers.ExceptionFilters
{
    public partial class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        public HttpGlobalExceptionFilter(IWebHostEnvironment env, ILogger logger)
        {
            _env = env;
            _logger = logger;
        }

        [Obsolete]
        public void OnException(ExceptionContext context)
        {
            Log.Error(context.Exception, context.Exception.InnerException?.Message);
            _logger.Error(context.Exception, context.Exception.InnerException?.Message);

            if (context.Exception.GetType() == typeof(DomainException))
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] { context.Exception.Message }
                };

                context.Result = new BadRequestObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[]
                        {"Oops, an error occured. Trace Identifier: " + context.HttpContext?.TraceIdentifier ?? "N/A"}
                };

                if (_env.EnvironmentName == EnvironmentName.Development ||
                    _env.EnvironmentName.ToUpper() == "LocalDevelopment".ToUpper())
                    json.DeveloperMessage = context.Exception;

                context.Result = new InternalServerErrorObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }
    }
}