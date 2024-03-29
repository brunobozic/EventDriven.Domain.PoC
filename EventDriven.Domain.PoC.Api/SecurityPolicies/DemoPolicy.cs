﻿using System;
using System.Linq;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace EventDriven.Domain.PoC.Api.Rest.SecurityPolicies
{
    public class DemoPolicy : AuthorizationHandler<DemoRequirement, Guid>
    {
        private readonly IServiceProvider _serviceProvider;


        public DemoPolicy(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            DemoRequirement requirement, Guid resource)
        {
            var user = context.User;

            var superUserClaim = user.Claims.First(p => p.Type == JwtClaimNameConstants.SUPERUSER_CLAIM_NAME);

            if (bool.TryParse(superUserClaim.Value, out var isSuperUser) && isSuperUser) context.Succeed(requirement);
        }
    }
}