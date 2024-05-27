using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContexts;
using IdentityService.Domain.DomainEntities;
using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService.Data.Seed;

public static class IdentitySeed
{
    private const int NUM_MONTHS_ACTIVE_FOR_USERS = 24;
    private const int NUM_MONTHS_ACTIVE_FOR_ROLES = 24;
    private const int NUM_MONTHS_ACTIVE_FOR_CODEBOOKS = 24;
    private const int NUM_MONTHS_ACTIVE_FOR_PERMISSIONS = 24;
    private const int NUM_MONTHS_ACTIVE_FOR_RESOURCES = 24;
    private const int NUM_MONTHS_ACTIVE_FOR_ROLE_PERMISSIONS = 24;
    public static async Task SeedSystemUserAsync(ApplicationDbContext context)
    {

        // Define the SystemUser's unique characteristics
        var systemUserName = "system.user";
        var systemUserEmail = "system.user@system.com";

        // Check if the SystemUser already exists to ensure idempotence
        if (await context.ApplicationUsers.Where(u => u.UserName == systemUserName).SingleAsync() == null)
        {
            var systemUser = User.NewActiveWithPasswordAndEmailVerified(Guid.NewGuid(), systemUserEmail, systemUserName, "System", "User", "83797858", DateTimeOffset.UtcNow.AddYears(-30), DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddMonths(12), "SecurePassword!123", null, "seed origin", true);


            //if (!createUserResult.Succeeded)
            //    throw new ApplicationException(
            //        $"Failed to seed the SystemUser: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");

            //var assignRoleResult =
            //    await userManager.AddToRoleAsync(systemUser, "SUPER_ADMIN");

            //if (!assignRoleResult.Succeeded)
            //    throw new ApplicationException(
            //        $"Failed to assign roles to the SystemUser: {string.Join(", ", assignRoleResult.Errors.Select(e => e.Description))}");
        }
    }

    public static async Task<bool> SeedCountriesAsync(ApplicationDbContext context)
    {
        // Define an array of countries to add
        var countriesToAdd = new List<CountryCodebook>
        {
            new()
            {
                Name = "Croatia",
                LocalName = "Hrvatska",
                ISO_3166_ALPHA_2 = "HR",
                ISO_3166_ALPHA_3 = "HRV",
                ISO_3166_NUMERIC = "191",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Hungary",
                LocalName = "Magyarország",
                ISO_3166_ALPHA_2 = "HU",
                ISO_3166_ALPHA_3 = "HUN",
                ISO_3166_NUMERIC = "348",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Poland",
                LocalName = "Polska",
                ISO_3166_ALPHA_2 = "PL",
                ISO_3166_ALPHA_3 = "POL",
                ISO_3166_NUMERIC = "616",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Slovenia",
                LocalName = "Slovenija",
                ISO_3166_ALPHA_2 = "SI",
                ISO_3166_ALPHA_3 = "SVN",
                ISO_3166_NUMERIC = "705",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "France",
                LocalName = "France",
                ISO_3166_ALPHA_2 = "FR",
                ISO_3166_ALPHA_3 = "FRA",
                ISO_3166_NUMERIC = "250",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Germany",
                LocalName = "Deutschland",
                ISO_3166_ALPHA_2 = "DE",
                ISO_3166_ALPHA_3 = "DEU",
                ISO_3166_NUMERIC = "276",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Italy",
                LocalName = "Italia",
                ISO_3166_ALPHA_2 = "IT",
                ISO_3166_ALPHA_3 = "ITA",
                ISO_3166_NUMERIC = "380",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Spain",
                LocalName = "España",
                ISO_3166_ALPHA_2 = "ES",
                ISO_3166_ALPHA_3 = "ESP",
                ISO_3166_NUMERIC = "724",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Portugal",
                LocalName = "Portugal",
                ISO_3166_ALPHA_2 = "PT",
                ISO_3166_ALPHA_3 = "PRT",
                ISO_3166_NUMERIC = "620",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Netherlands",
                LocalName = "Nederland",
                ISO_3166_ALPHA_2 = "NL",
                ISO_3166_ALPHA_3 = "NLD",
                ISO_3166_NUMERIC = "528",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Belgium",
                LocalName = "België",
                ISO_3166_ALPHA_2 = "BE",
                ISO_3166_ALPHA_3 = "BEL",
                ISO_3166_NUMERIC = "056",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Sweden",
                LocalName = "Sverige",
                ISO_3166_ALPHA_2 = "SE",
                ISO_3166_ALPHA_3 = "SWE",
                ISO_3166_NUMERIC = "752",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Norway",
                LocalName = "Norge",
                ISO_3166_ALPHA_2 = "NO",
                ISO_3166_ALPHA_3 = "NOR",
                ISO_3166_NUMERIC = "578",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Finland",
                LocalName = "Suomi",
                ISO_3166_ALPHA_2 = "FI",
                ISO_3166_ALPHA_3 = "FIN",
                ISO_3166_NUMERIC = "246",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Denmark",
                LocalName = "Danmark",
                ISO_3166_ALPHA_2 = "DK",
                ISO_3166_ALPHA_3 = "DNK",
                ISO_3166_NUMERIC = "208",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Ireland",
                LocalName = "Éire",
                ISO_3166_ALPHA_2 = "IE",
                ISO_3166_ALPHA_3 = "IRL",
                ISO_3166_NUMERIC = "372",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Greece",
                LocalName = "Ελλάδα",
                ISO_3166_ALPHA_2 = "GR",
                ISO_3166_ALPHA_3 = "GRC",
                ISO_3166_NUMERIC = "300",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Czech Republic",
                LocalName = "Česká republika",
                ISO_3166_ALPHA_2 = "CZ",
                ISO_3166_ALPHA_3 = "CZE",
                ISO_3166_NUMERIC = "203",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Slovakia",
                LocalName = "Slovensko",
                ISO_3166_ALPHA_2 = "SK",
                ISO_3166_ALPHA_3 = "SVK",
                ISO_3166_NUMERIC = "703",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Austria",
                LocalName = "Österreich",
                ISO_3166_ALPHA_2 = "AT",
                ISO_3166_ALPHA_3 = "AUT",
                ISO_3166_NUMERIC = "040",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Switzerland",
                LocalName = "Schweiz",
                ISO_3166_ALPHA_2 = "CH",
                ISO_3166_ALPHA_3 = "CHE",
                ISO_3166_NUMERIC = "756",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Luxembourg",
                LocalName = "Luxembourg",
                ISO_3166_ALPHA_2 = "LU",
                ISO_3166_ALPHA_3 = "LUX",
                ISO_3166_NUMERIC = "442",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Liechtenstein",
                LocalName = "Liechtenstein",
                ISO_3166_ALPHA_2 = "LI",
                ISO_3166_ALPHA_3 = "LIE",
                ISO_3166_NUMERIC = "438",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Monaco",
                LocalName = "Monaco",
                ISO_3166_ALPHA_2 = "MC",
                ISO_3166_ALPHA_3 = "MCO",
                ISO_3166_NUMERIC = "492",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "San Marino",
                LocalName = "San Marino",
                ISO_3166_ALPHA_2 = "SM",
                ISO_3166_ALPHA_3 = "SMR",
                ISO_3166_NUMERIC = "674",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Malta",
                LocalName = "Malta",
                ISO_3166_ALPHA_2 = "MT",
                ISO_3166_ALPHA_3 = "MLT",
                ISO_3166_NUMERIC = "470",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Andorra",
                LocalName = "Andorra",
                ISO_3166_ALPHA_2 = "AD",
                ISO_3166_ALPHA_3 = "AND",
                ISO_3166_NUMERIC = "020",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Bulgaria",
                LocalName = "България",
                ISO_3166_ALPHA_2 = "BG",
                ISO_3166_ALPHA_3 = "BGR",
                ISO_3166_NUMERIC = "100",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Romania",
                LocalName = "România",
                ISO_3166_ALPHA_2 = "RO",
                ISO_3166_ALPHA_3 = "ROU",
                ISO_3166_NUMERIC = "642",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Cyprus",
                LocalName = "Κύπρος",
                ISO_3166_ALPHA_2 = "CY",
                ISO_3166_ALPHA_3 = "CYP",
                ISO_3166_NUMERIC = "196",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Latvia",
                LocalName = "Latvija",
                ISO_3166_ALPHA_2 = "LV",
                ISO_3166_ALPHA_3 = "LVA",
                ISO_3166_NUMERIC = "428",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Lithuania",
                LocalName = "Lietuva",
                ISO_3166_ALPHA_2 = "LT",
                ISO_3166_ALPHA_3 = "LTU",
                ISO_3166_NUMERIC = "440",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Estonia",
                LocalName = "Eesti",
                ISO_3166_ALPHA_2 = "EE",
                ISO_3166_ALPHA_3 = "EST",
                ISO_3166_NUMERIC = "233",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Moldova",
                LocalName = "Moldova",
                ISO_3166_ALPHA_2 = "MD",
                ISO_3166_ALPHA_3 = "MDA",
                ISO_3166_NUMERIC = "498",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Albania",
                LocalName = "Shqipëria",
                ISO_3166_ALPHA_2 = "AL",
                ISO_3166_ALPHA_3 = "ALB",
                ISO_3166_NUMERIC = "008",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Kosovo",
                LocalName = "Kosovë",
                ISO_3166_ALPHA_2 = "XK",
                ISO_3166_ALPHA_3 = "XKX",
                ISO_3166_NUMERIC = "0", // Note: Kosovo is not assigned a numeric code in ISO 3166-1.
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Montenegro",
                LocalName = "Crna Gora",
                ISO_3166_ALPHA_2 = "ME",
                ISO_3166_ALPHA_3 = "MNE",
                ISO_3166_NUMERIC = "499",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "North Macedonia",
                LocalName = "Северна Македонија",
                ISO_3166_ALPHA_2 = "MK",
                ISO_3166_ALPHA_3 = "MKD",
                ISO_3166_NUMERIC = "807",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Serbia",
                LocalName = "Srbija",
                ISO_3166_ALPHA_2 = "RS",
                ISO_3166_ALPHA_3 = "SRB",
                ISO_3166_NUMERIC = "688",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Bosnia and Herzegovina",
                LocalName = "Bosna i Hercegovina",
                ISO_3166_ALPHA_2 = "BA",
                ISO_3166_ALPHA_3 = "BIH",
                ISO_3166_NUMERIC = "070",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Iceland",
                LocalName = "Ísland",
                ISO_3166_ALPHA_2 = "IS",
                ISO_3166_ALPHA_3 = "ISL",
                ISO_3166_NUMERIC = "352",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Belarus",
                LocalName = "Беларусь",
                ISO_3166_ALPHA_2 = "BY",
                ISO_3166_ALPHA_3 = "BLR",
                ISO_3166_NUMERIC = "112",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Ukraine",
                LocalName = "Україна",
                ISO_3166_ALPHA_2 = "UA",
                ISO_3166_ALPHA_3 = "UKR",
                ISO_3166_NUMERIC = "804",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Russia",
                LocalName = "Россия",
                ISO_3166_ALPHA_2 = "RU",
                ISO_3166_ALPHA_3 = "RUS",
                ISO_3166_NUMERIC = "643",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Turkey",
                LocalName = "Türkiye",
                ISO_3166_ALPHA_2 = "TR",
                ISO_3166_ALPHA_3 = "TUR",
                ISO_3166_NUMERIC = "792",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Georgia",
                LocalName = "საქართველო",
                ISO_3166_ALPHA_2 = "GE",
                ISO_3166_ALPHA_3 = "GEO",
                ISO_3166_NUMERIC = "268",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Armenia",
                LocalName = "Հայաստան",
                ISO_3166_ALPHA_2 = "AM",
                ISO_3166_ALPHA_3 = "ARM",
                ISO_3166_NUMERIC = "051",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            },
            new()
            {
                Name = "Azerbaijan",
                LocalName = "Azərbaycan",
                ISO_3166_ALPHA_2 = "AZ",
                ISO_3166_ALPHA_3 = "AZE",
                ISO_3166_NUMERIC = "031",
                IsActive = true,
                ActiveFrom = DateTimeOffset.UtcNow,
                ActiveTo = DateTimeOffset.UtcNow.AddMonths(NUM_MONTHS_ACTIVE_FOR_CODEBOOKS)
            }
        };

        // Iterate through each country to add
        foreach (var countryToAdd in countriesToAdd)
            // Check if the country already exists
            if (await context.Countries.AsQueryable().FirstOrDefaultAsync(i => i.Name == countryToAdd.Name) == null)
            {
                context.Add(countryToAdd);
                var res = await context.SaveChangesAsync();
            }

        return true;
    }



    public static async Task<bool> SeedUsersAsync(ApplicationDbContext myDbContext, IMyUnitOfWork myUnitOfWork)
    {
        if (!myDbContext.ApplicationUsers.Any(
                user => user.UserName == ApplicationWideConstants.SYSTEM_USER_USERNAME))
        {
            var newUser = User.NewActiveWithPasswordAndEmailVerified(
                Guid.Parse(ApplicationWideConstants.SYSTEM_USER)
                , ApplicationWideConstants.USER_EMAIL
                , ApplicationWideConstants.SYSTEM_USER_USERNAME
                , ApplicationWideConstants.SYSTEM_USER_NAME
                , ApplicationWideConstants.SYSTEM_USER_SURNAME
                , ApplicationWideConstants.SYSTEM_USER_OIB
                , DateTimeOffset.UtcNow.AddYears(-200)
                , DateTimeOffset.UtcNow
                , DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.SYSTEM_USER_ACTIVE_TO_ADD_YEARS)
                , ApplicationWideConstants.SYSTEM_USER_PASSWORD
                , null
                , ""
                , true
            );

            var result = await myDbContext.ApplicationUsers.AddAsync(newUser);

            await myUnitOfWork.SaveChangesAsync();
        }

        var creatorUser =
            myDbContext.ApplicationUsers.FirstOrDefault(u =>
                u.NormalizedUserName == ApplicationWideConstants.SYSTEM_USER_USERNAME.ToUpper());

        if (!myDbContext.ApplicationUsers.Any(user => user.UserName == ApplicationWideConstants.USER_EMAIL))
        {
            var newUser = User.NewActiveWithPasswordAndEmailVerified(
                Guid.NewGuid()
                , ApplicationWideConstants.USER_EMAIL
                , ApplicationWideConstants.USER_EMAIL
                , "Bruno"
                , "Bozic"
                , "1111112"
                , DateTimeOffset.UtcNow.AddYears(-41)
                , DateTimeOffset.UtcNow
                , DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                , ApplicationWideConstants.SEED_PASSWORD
                , creatorUser
                , ""
                , false
            );

            var result = await myDbContext.ApplicationUsers.AddAsync(newUser);

            await myUnitOfWork.SaveChangesAsync();
        }

        if (myDbContext.ApplicationRoles.FirstOrDefault(r => r.Name == ApplicationWideConstants.GUEST) == null)
        {
            var role = Role.NewActiveDraft(ApplicationWideConstants.GUEST, "Guest role", DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddYears(1), creatorUser);

            var result = await myDbContext.ApplicationRoles.AddAsync(role);

            await myUnitOfWork.SaveChangesAsync();
        }

        if (myDbContext.ApplicationRoles.FirstOrDefault(r =>
                r.Name == ApplicationWideConstants.ADMINISTRATOR_ROLE_NAME) == null)
        {
            var role = Role.NewActiveDraft(ApplicationWideConstants.ADMINISTRATOR_ROLE_NAME, "Admin role",
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddYears(1), creatorUser);

            var result = await myDbContext.ApplicationRoles.AddAsync(role);

            await myUnitOfWork.SaveChangesAsync();
        }

        #region Administrators, adding roles

        if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "BrunoBozic"))
        {
            var newUser = User.NewActiveWithPasswordAndEmailVerified(
                Guid.NewGuid()
                , ApplicationWideConstants.USER_EMAIL
                , "BrunoBozic"
                , "Bruno"
                , "Bozic"
                , "1111113"
                , DateTimeOffset.UtcNow.AddYears(-41)
                , DateTimeOffset.UtcNow
                , DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                , ApplicationWideConstants.SEED_PASSWORD
                , creatorUser
                , ""
                , false
            );

            var applicationRole = myDbContext.ApplicationRoles
                .Where(r => r.Name.ToUpper() == "ADMINISTRATOR")
                .Select(u => u).FirstOrDefault();

            await myDbContext.ApplicationUsers.AddAsync(newUser);

            await myUnitOfWork.SaveChangesAsync();

            var usr = await myDbContext.ApplicationUsers.AsQueryable().Where(u => u.UserName == "BrunoBozic")
                .FirstOrDefaultAsync();

            usr.AddRole(applicationRole, creatorUser);

            await myUnitOfWork.SaveChangesAsync();
        }

        if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "testadmin2"))
        {
            var newUser2 = User.NewActiveWithPasswordAndEmailVerified(
                Guid.NewGuid()
                , ApplicationWideConstants.UserEmail2
                , "testadmin2" // Username
                , "Bruno"
                , "Bozic"
                , "22222222" // OIB
                , DateTimeOffset.UtcNow.AddYears(-41)
                , DateTimeOffset.UtcNow
                , DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                , ApplicationWideConstants.SEED_PASSWORD
                , creatorUser
                , ""
                , false
            );

            var applicationRole2 = myDbContext.ApplicationRoles.Where(r => r.Name.ToUpper() == "ADMINISTRATOR")
                .Select(u => u).FirstOrDefault();

            await myDbContext.ApplicationUsers.AddAsync(newUser2);

            await myUnitOfWork.SaveChangesAsync();

            var usr = await myDbContext.ApplicationUsers.AsQueryable().Where(u => u.UserName == "testadmin2")
                .FirstOrDefaultAsync();

            usr.AddRole(applicationRole2, creatorUser);

            await myUnitOfWork.SaveChangesAsync();
        }

        if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "testadmin3"))
        {
            var newUser3 = User.NewActiveWithPasswordAndEmailVerified(
                Guid.NewGuid()
                , ApplicationWideConstants.UserEmail3
                , "testadmin3"
                , "Test"
                , "Admin3"
                , "3333333" // OIB
                , DateTimeOffset.UtcNow.AddYears(-41)
                , DateTimeOffset.UtcNow
                , DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                , ApplicationWideConstants.SEED_PASSWORD
                , creatorUser
                , ""
                , false
            );

            var applicationRole3 = myDbContext.ApplicationRoles.Where(r => r.Name.ToUpper() == "ADMINISTRATOR")
                .Select(u => u).FirstOrDefault();

            await myDbContext.ApplicationUsers.AddAsync(newUser3);

            await myUnitOfWork.SaveChangesAsync();

            var usr = await myDbContext.ApplicationUsers.AsQueryable().Where(u => u.UserName == "testadmin3")
                .FirstOrDefaultAsync();

            usr.AddRole(applicationRole3, creatorUser);

            await myUnitOfWork.SaveChangesAsync();
        }

        if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "testadmin4"))
        {
            var newUser4 = User.NewActiveWithPasswordAndEmailVerified(
                Guid.NewGuid()
                , ApplicationWideConstants.UserEmail4
                , "testadmin4"
                , "Test"
                , "Admin4"
                , "44444444" // OIB
                , DateTimeOffset.UtcNow.AddYears(-41)
                , DateTimeOffset.UtcNow
                , DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                , ApplicationWideConstants.SEED_PASSWORD
                , creatorUser
                , ""
                , false
            );

            var applicationRole4 = myDbContext.ApplicationRoles.Where(r => r.Name.ToUpper() == "ADMINISTRATOR")
                .Select(u => u).FirstOrDefault();

            await myDbContext.ApplicationUsers.AddAsync(newUser4);

            await myUnitOfWork.SaveChangesAsync();

            var usr = await myDbContext.ApplicationUsers.AsQueryable().Where(u => u.UserName == "testadmin4")
                .FirstOrDefaultAsync();

            usr.AddRole(applicationRole4, creatorUser);

            await myUnitOfWork.SaveChangesAsync();
        }

        #endregion Administrators, adding roles

        #region Address Types

        if (myDbContext.AddressTypes.FirstOrDefault(r =>
                r.Name == AddressTypeEnum.Primary.ToDescriptionString()) == null)
        {
            var addressType = AddressType.NewActiveDraft(
                AddressTypeEnum.Primary.ToDescriptionString()
                , "Primary address"
                , creatorUser
                , DateTimeOffset.UtcNow
                , DateTimeOffset.Now.AddYears(ApplicationWideConstants.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES)
            );

            var result = await myDbContext.AddressTypes.AddAsync(addressType);

            await myUnitOfWork.SaveChangesAsync();
        }

        if (myDbContext.AddressTypes.FirstOrDefault(r =>
                r.Name == AddressTypeEnum.Secondary.ToDescriptionString()) == null)
        {
            var addressType = AddressType.NewActiveDraft(
                AddressTypeEnum.Secondary.ToDescriptionString()
                , "Secondary address"
                , creatorUser
                , DateTimeOffset.UtcNow
                , DateTimeOffset.Now.AddYears(ApplicationWideConstants.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES)
            );

            var result = await myDbContext.AddressTypes.AddAsync(addressType);

            await myUnitOfWork.SaveChangesAsync();
        }

        if (myDbContext.AddressTypes.FirstOrDefault(r => r.Name == AddressTypeEnum.Living.ToDescriptionString()) ==
            null)
        {
            var addressType = AddressType.NewActiveDraft(
                AddressTypeEnum.Living.ToDescriptionString()
                , "Living address"
                , creatorUser
                , DateTimeOffset.UtcNow
                , DateTimeOffset.Now.AddYears(ApplicationWideConstants.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES)
            );

            var result = await myDbContext.AddressTypes.AddAsync(addressType);

            await myUnitOfWork.SaveChangesAsync();
        }

        #endregion Address Types

        return true;
    }
}