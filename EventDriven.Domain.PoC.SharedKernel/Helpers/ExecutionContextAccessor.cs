using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.AspNetCore.Http;
using System;

namespace EventDriven.Domain.PoC.SharedKernel.Helpers
{
    public class ExecutionContextAccessor : IExecutionContextAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid CorrelationId =>
            //if (IsAvailable && _httpContextAccessor.HttpContext.Request.Headers.Keys.Any(x => x == CorrelationMiddleware.CorrelationHeaderKey))
            //{
            //    return Guid.Parse(
            //        _httpContextAccessor.HttpContext.Request.Headers[CorrelationMiddleware.CorrelationHeaderKey]);
            //}
            //throw new ApplicationException("Http context and correlation id is not available");
            Guid.NewGuid();

        public bool IsAvailable => _httpContextAccessor.HttpContext != null;
    }
}