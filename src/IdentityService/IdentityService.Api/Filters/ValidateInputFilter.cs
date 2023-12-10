using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using SharedKernel.DomainImplementations.DomainErrors;

namespace IdentityService.Api.Filters;

public class ValidateInputFilter : IActionFilter
{
    #region Constructor

    #endregion Constructor

    #region

    #endregion

    #region IActionFilter Implementation

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // This filter doesn't do anything post action.
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
            return;

        Log.Warning("{0} => Model validation failed for {@Input} with validation {@Errors}",
            DateTime.UtcNow.ToLocalTime(),
            context.ActionArguments,
            context.ModelState?
                .SelectMany(kvp => kvp.Value.Errors)
                .Select(e => e.ErrorMessage));

        context.Result = new BadRequestObjectResult(
            from kvp in context.ModelState
            from e in kvp.Value.Errors
            let k = kvp.Key
            select new ValidationError(ValidationError.Type.Input, null, k, e.ErrorMessage));
    }

    #endregion
}