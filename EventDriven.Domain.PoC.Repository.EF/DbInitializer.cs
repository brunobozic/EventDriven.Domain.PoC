using System.Threading.Tasks;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace EventDriven.Domain.PoC.Repository.EF
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext myDbContext)
        {
            // myDbContext.Database.EnsureCreated();
            await myDbContext.Database.MigrateAsync();
            await myDbContext.SaveChangesAsync();
        }
    }
}