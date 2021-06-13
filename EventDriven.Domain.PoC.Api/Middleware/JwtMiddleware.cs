using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EventDriven.Domain.PoC.Api.Rest.Middleware
{
    public class JwtMiddleware
    {
        private readonly MyConfigurationValues _appSettings;
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next, IOptions<MyConfigurationValues> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, DbContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachapplicationUserToContext(context, dataContext, token);

            await _next(context);
        }

        private async Task AttachapplicationUserToContext(HttpContext context, DbContext dataContext, string token)
        {
            var myDbContext = dataContext as ApplicationDbContext;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken) validatedToken;
                var applicationUserId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach applicationUser to context on successful jwt validation
                var user = myDbContext.ApplicationUsers.Where(user => user.Id == applicationUserId).SingleOrDefault();

                context.Items["ApplicationUser"] = user;
            }
            catch
            {
                // do nothing if jwt validation fails
                // applicationUser is not attached to context so request won't have access to secure routes
            }
        }
    }
}