using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventDriven.Domain.PoC.Api.Rest.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
                filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
        }
    }
}