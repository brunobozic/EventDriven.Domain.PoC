using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Data.DatabaseContext;
using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Extensions;

namespace IdentityService.Data.Seed;

public static class IdentitySeed
{
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