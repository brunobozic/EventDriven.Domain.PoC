using System;
using System.Linq;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext;
using EventDriven.Domain.PoC.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventDriven.Domain.PoC.Repository.EF.Seed
{
    public static class IdentitySeed
    {
        public static async Task SeedUsersAsync(ApplicationDbContext myDbContext, IMyUnitOfWork myUnitOfWork)
        {
            if (!myDbContext.ApplicationUsers.Any(user => user.UserName == Consts.SYSTEM_USER_USERNAME))
            {
                var newUser = User.NewActiveWithPasswordAndEmailVerified(
                    Consts.UserEmail
                    , Consts.SYSTEM_USER_USERNAME
                    , "System"
                    , "System"
                    , Consts.SYSTEM_USER_OIB
                    , DateTimeOffset.UtcNow.AddYears(-200)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(Consts.SYSTEM_USER_ACTIVE_TO_ADD_YEARS)
                    , Consts.SYSTEM_USER_PASSWORD
                    , null
                    , ""
                    , true
                );

                var result = await myDbContext.ApplicationUsers.AddAsync(newUser);

                await myUnitOfWork.SaveChangesAsync();
            }

            var creatorUser = myDbContext.ApplicationUsers.SingleOrDefault(u => u.NormalizedUserName == Consts.SYSTEM_USER_USERNAME.ToUpper());

            if (!myDbContext.ApplicationUsers.Any(user => user.UserName == Consts.UserEmail))
            {
                var newUser = User.NewActiveWithPasswordAndEmailVerified(
                    Consts.UserEmail
                    , Consts.UserEmail
                    , "Bruno"
                    , "Bozic"
                    , "1111112"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(Consts.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                    , "Pwd01!"
                    , creatorUser
                    , ""
                    , false
                );

                var result = await myDbContext.ApplicationUsers.AddAsync(newUser);

                await myUnitOfWork.SaveChangesAsync();
            }


            if (myDbContext.ApplicationRoles.SingleOrDefault(r => r.Name == Consts.Guest) == null)
            {
                var role = Role.NewActiveDraft(Consts.Guest, "Guest role", DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow.AddYears(1), creatorUser);

                var result = await myDbContext.ApplicationRoles.AddAsync(role);

                await myUnitOfWork.SaveChangesAsync();
            }

            if (myDbContext.ApplicationRoles.SingleOrDefault(r => r.Name == Consts.AdministratorRoleName) == null)
            {
                var role = Role.NewActiveDraft(Consts.AdministratorRoleName, "Admin role", DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow.AddYears(1), creatorUser);

                var result = await myDbContext.ApplicationRoles.AddAsync(role);

                await myUnitOfWork.SaveChangesAsync();
            }

            #region Administrators, adding roles

            if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "BrunoBozic"))
            {
                var newUser = User.NewActiveWithPasswordAndEmailVerified(
                    Consts.UserEmail
                    , "BrunoBozic"
                    , "Bruno"
                    , "Bozic"
                    , "1111113"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(Consts.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                    , "Pwd01!"
                    , creatorUser
                    , ""
                    , false
                );

                var applicationRole = myDbContext.ApplicationRoles
                    .Where(r => r.Name.ToUpper() == "ADMINISTRATOR")
                    .Select(u => u).SingleOrDefault();

                await myDbContext.ApplicationUsers.AddAsync(newUser);

                await myUnitOfWork.SaveChangesAsync();

                var usr = await myDbContext.ApplicationUsers.AsQueryable().Where(u => u.UserName == "BrunoBozic")
                    .SingleOrDefaultAsync();

                usr.AddRole(applicationRole, creatorUser);

                await myUnitOfWork.SaveChangesAsync();
            }

            if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "testadmin2"))
            {
                var newUser2 = User.NewActiveWithPasswordAndEmailVerified(
                    Consts.UserEmail2
                    , "testadmin2"
                    , "Bruno"
                    , "Bozic"
                    , "22222222"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(Consts.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                    , "Pwd01!"
                    , creatorUser
                    , ""
                    , false
                );

                var applicationRole2 = myDbContext.ApplicationRoles.Where(r => r.Name.ToUpper() == "ADMINISTRATOR")
                    .Select(u => u).SingleOrDefault();

                await myDbContext.ApplicationUsers.AddAsync(newUser2);

                await myUnitOfWork.SaveChangesAsync();

                var usr = await myDbContext.ApplicationUsers.AsQueryable().Where(u => u.UserName == "testadmin2")
                    .SingleOrDefaultAsync();

                usr.AddRole(applicationRole2, creatorUser);

                await myUnitOfWork.SaveChangesAsync();
            }

            if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "testadmin3"))
            {
                var newUser3 = User.NewActiveWithPasswordAndEmailVerified(
                    Consts.UserEmail3
                    , "testadmin3"
                    , "Test"
                    , "Admin3"
                    , "3333333"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(Consts.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                    , "Pwd01!"
                    , creatorUser
                    , ""
                    , false
                );

                var applicationRole3 = myDbContext.ApplicationRoles.Where(r => r.Name.ToUpper() == "ADMINISTRATOR")
                    .Select(u => u).SingleOrDefault();

                await myDbContext.ApplicationUsers.AddAsync(newUser3);

                await myUnitOfWork.SaveChangesAsync();

                var usr = await myDbContext.ApplicationUsers.AsQueryable().Where(u => u.UserName == "testadmin3")
                    .SingleOrDefaultAsync();

                usr.AddRole(applicationRole3, creatorUser);

                await myUnitOfWork.SaveChangesAsync();
            }

            if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "testadmin4"))
            {
                var newUser4 = User.NewActiveWithPasswordAndEmailVerified(
                    Consts.UserEmail4
                    , "testadmin4"
                    , "Test"
                    , "Admin4"
                    , "44444444"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(Consts.DEMO_USER_ACTIVE_TO_ADD_YEARS)
                    , "Pwd01!"
                    , creatorUser
                    , ""
                    , false
                );

                var applicationRole4 = myDbContext.ApplicationRoles.Where(r => r.Name.ToUpper() == "ADMINISTRATOR")
                    .Select(u => u).SingleOrDefault();

                await myDbContext.ApplicationUsers.AddAsync(newUser4);

                await myUnitOfWork.SaveChangesAsync();

                var usr = await myDbContext.ApplicationUsers.AsQueryable().Where(u => u.UserName == "testadmin4")
                    .SingleOrDefaultAsync();

                usr.AddRole(applicationRole4, creatorUser);

                await myUnitOfWork.SaveChangesAsync();
            }

            #endregion Administrators, adding roles

            #region Address Types

            if (myDbContext.AddressTypes.SingleOrDefault(r =>
                r.Name == AddressTypeEnum.Primary.ToDescriptionString()) == null)
            {
                var addressType = AddressType.NewActiveDraft(
                    AddressTypeEnum.Primary.ToDescriptionString()
                    , "Primary address"
                    , creatorUser
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.Now.AddYears(Consts.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES)
                );

                var result = await myDbContext.AddressTypes.AddAsync(addressType);

                await myUnitOfWork.SaveChangesAsync();
            }

            if (myDbContext.AddressTypes.SingleOrDefault(r =>
                r.Name == AddressTypeEnum.Secondary.ToDescriptionString()) == null)
            {
                var addressType = AddressType.NewActiveDraft(
                    AddressTypeEnum.Secondary.ToDescriptionString()
                    , "Secondary address"
                    , creatorUser
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.Now.AddYears(Consts.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES)
                );

                var result = await myDbContext.AddressTypes.AddAsync(addressType);

                await myUnitOfWork.SaveChangesAsync();
            }

            if (myDbContext.AddressTypes.SingleOrDefault(r => r.Name == AddressTypeEnum.Living.ToDescriptionString()) ==
                null)
            {
                var addressType = AddressType.NewActiveDraft(
                    AddressTypeEnum.Living.ToDescriptionString()
                    , "Living address"
                    , creatorUser
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.Now.AddYears(Consts.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES)
                );

                var result = await myDbContext.AddressTypes.AddAsync(addressType);

                await myUnitOfWork.SaveChangesAsync();
            }

            #endregion Address Types
        }
    }
}