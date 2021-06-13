using System;
using System.Collections.Generic;
using EventDriven.Domain.PoC.SharedKernel.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventDriven.Domain.PoC.Api.Rest.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<RoleEnum> _roles;

        public MyAuthorizeAttribute(params RoleEnum[] roles)
        {
            _roles = roles ?? new RoleEnum[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var applicationUser = (ApplicationUser)context.HttpContext.Items["ApplicationUser"];
            //if (applicationUser == null || (_roles.Any() && !_roles.Contains(applicationUser.UserRoles.Select(r => r.Role.Name).ToList())
            //{
            //    // not logged in or role not authorized
            //    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            //}
        }
    }
}