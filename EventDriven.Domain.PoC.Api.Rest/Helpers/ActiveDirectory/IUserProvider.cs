using EventDriven.Domain.PoC.SharedKernel.ActiveDirectory;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

namespace EventDriven.Domain.PoC.Api.Rest.Helpers.ActiveDirectory
{
    public interface IUserProvider
    {
        AdUser CurrentUser { get; set; }
        bool Initialized { get; set; }

        Task Create(HttpContext context, IConfiguration config, IWebHostEnvironment env);

        Task<AdUser> GetAdUser(IIdentity identity, IConfiguration config);

        Task<AdUser> GetAdUser(string samapplicationUserName);

        Task<AdUser> GetAdUser(Guid guid);

        Task<List<AdUser>> GetDomainUsers();

        Task<List<AdUser>> FindDomainUser(string search);
    }
}