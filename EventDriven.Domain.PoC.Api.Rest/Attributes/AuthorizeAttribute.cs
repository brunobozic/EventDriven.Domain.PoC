
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventDriven.Domain.PoC.Api.Rest.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<string> _roles;

        public MyAuthorizeAttribute(params string[] roles)
        {
            _roles = roles ?? new string[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var applicationUser = (User)context.HttpContext.Items["ApplicationUser"];
            var rolesOnUser = applicationUser.GetUserRoles().Select(r => r.Name).ToArray();

            if (applicationUser != null && _roles.Any())
            {
                var isAuth = false;
                foreach (var role in rolesOnUser)
                    if (_roles.Contains(role))
                        isAuth = true;

                if (!isAuth)
                    // not logged in or role not authorized
                    context.Result = new JsonResult(new { message = "Unauthorized" })
                    { StatusCode = StatusCodes.Status401Unauthorized };
            }

            // not logged in or role not authorized
            context.Result = new JsonResult(new { message = "Unauthorized" })
            { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}