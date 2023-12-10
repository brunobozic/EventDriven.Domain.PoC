using System.Threading.Tasks;
using IdentityService.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext myDbContext)
    {
        // myDbContext.Database.EnsureCreated();
        await myDbContext.Database.MigrateAsync();
        await myDbContext.SaveChangesAsync();
    }
}