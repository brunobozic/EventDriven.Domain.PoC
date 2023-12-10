using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SharedKernel.ActiveDirectory;

namespace IdentityService.Api.Helpers.ActiveDirectory;

public interface IUserProvider
{
    AdUser CurrentUser { get; set; }
    bool Initialized { get; set; }

    Task Create(HttpContext context, IConfiguration config, IWebHostEnvironment env);

    Task<List<AdUser>> FindDomainUser(string search);

    Task<AdUser> GetAdUser(IIdentity identity, IConfiguration config);

    Task<AdUser> GetAdUser(string samapplicationUserName);

    Task<AdUser> GetAdUser(Guid guid);

    Task<List<AdUser>> GetDomainUsers();
}