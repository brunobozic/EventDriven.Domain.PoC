using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContexts;
using IdentityService.Data.Seed;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

public class DatabaseInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly IMyUnitOfWork _unitOfWork;

    public DatabaseInitializer(ApplicationDbContext context, IMyUnitOfWork unitOfWork)
    {
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public void MigrateAndSeed()
    {
        if (IsUpdateDatabaseCommand())
        {
            // Skip seeding during update-database
            _context.Database.Migrate();
            return;
        }

        // Apply migrations and seed data
        _context.Database.Migrate();
        IdentitySeed.SeedUsersAsync(_context, _unitOfWork).Wait();
    }

    private bool IsUpdateDatabaseCommand()
    {
        var processName = Process.GetCurrentProcess().ProcessName;
        return processName.Equals("ef", StringComparison.OrdinalIgnoreCase) ||
               processName.Equals("dotnet-ef", StringComparison.OrdinalIgnoreCase);
    }
}
