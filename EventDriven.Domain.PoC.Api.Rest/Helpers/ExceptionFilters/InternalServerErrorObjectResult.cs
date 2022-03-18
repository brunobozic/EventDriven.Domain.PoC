using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventDriven.Domain.PoC.Api.Rest.Helpers.ExceptionFilters
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
            : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}