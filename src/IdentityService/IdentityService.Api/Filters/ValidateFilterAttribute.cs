using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityService.Api.Filters;

public class ValidateFilterAttribute : ActionFilterAttribute
{
    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!filterContext.ModelState.IsValid)
            filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
    }
}