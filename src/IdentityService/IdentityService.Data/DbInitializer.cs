using IdentityService.Data.DatabaseContexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IdentityService.Data;

public static class DbInitializer
{
    public async static Task InitializeAsync(ApplicationDbContext myDbContext)
    {
        // myDbContext.Database.EnsureCreated();
        await myDbContext.Database.MigrateAsync();
        await myDbContext.SaveChangesAsync();
    }
}

