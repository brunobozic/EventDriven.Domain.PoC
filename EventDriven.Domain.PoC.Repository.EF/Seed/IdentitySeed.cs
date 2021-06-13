using System;
using System.Linq;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork;
using EventDriven.Domain.PoC.Repository.EF.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace EventDriven.Domain.PoC.Repository.EF.Seed
{
    public static class IdentitySeed
    {
        private const string UserEmail = "test.admin@gmail.com";
        private const string UserEmail2 = "bruno.bozic@gmail.com";
        private const string UserEmail3 = "test.admin3@gmail.com";
        private const string UserEmail4 = "test.admin4@gmail.com";
        private const string AdministratorRoleName = "Administrator";
        private const string Guest = "Guest";

        public static async Task SeedUsersAsync(ApplicationDbContext myDbContext, IMyUnitOfWork myUnitOfWork)
        {
            if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "System"))
            {
                var newUser = User.NewActiveWithPasswordAndEmailVerified(
                    UserEmail
                    , "System"
                    , "System"
                    , "System"
                    , "1111111"
                    , DateTimeOffset.UtcNow.AddYears(-200)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(100)
                    , "System"
                );

                var result = await myDbContext.ApplicationUsers.AddAsync(newUser);

                await myUnitOfWork.SaveChangesAsync();
            }

            var creatorUser = myDbContext.ApplicationUsers.FirstOrDefault(u => u.Id == 1);

            if (!myDbContext.ApplicationUsers.Any(user => user.UserName == UserEmail))
            {
                var newUser = User.NewActiveWithPasswordAndEmailVerified(
                    UserEmail
                    , UserEmail
                    , "Bruno"
                    , "Bozic"
                    , "1111111"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(1)
                    , "Pwd01!"
                    , creatorUser
                );

                var result = await myDbContext.ApplicationUsers.AddAsync(newUser);

                await myUnitOfWork.SaveChangesAsync();
            }


            if (myDbContext.ApplicationRoles.SingleOrDefault(r => r.Name == Guest) == null)
            {
                var role = Role.NewActiveDraft(Guest, "Guest role", DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow.AddYears(1), creatorUser);

                var result = await myDbContext.ApplicationRoles.AddAsync(role);

                await myUnitOfWork.SaveChangesAsync();
            }

            if (myDbContext.ApplicationRoles.SingleOrDefault(r => r.Name == AdministratorRoleName) == null)
            {
                var role = Role.NewActiveDraft(AdministratorRoleName, "Admin role", DateTimeOffset.UtcNow,
                    DateTimeOffset.UtcNow.AddYears(1), creatorUser);

                var result = await myDbContext.ApplicationRoles.AddAsync(role);

                await myUnitOfWork.SaveChangesAsync();
            }

            #region Administrators, adding roles

            if (!myDbContext.ApplicationUsers.Any(user => user.UserName == "BrunoBozic"))
            {
                var newUser = User.NewActiveWithPasswordAndEmailVerified(
                    UserEmail
                    , "BrunoBozic"
                    , "Bruno"
                    , "Bozic"
                    , "1111111"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(1)
                    , "Pwd01!"
                    , creatorUser
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
                    UserEmail2
                    , "testadmin2"
                    , "Bruno"
                    , "Bozic"
                    , "22222222"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(1)
                    , "Pwd01!"
                    , creatorUser
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
                    UserEmail3
                    , "testadmin3"
                    , "Test"
                    , "Admin3"
                    , "3333333"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(1)
                    , "Pwd01!"
                    , creatorUser
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
                    UserEmail4
                    , "testadmin4"
                    , "Test"
                    , "Admin4"
                    , "44444444"
                    , DateTimeOffset.UtcNow.AddYears(-41)
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow.AddYears(1)
                    , "Pwd01!"
                    , creatorUser
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

            if (myDbContext.AddressTypes.SingleOrDefault(r => r.Name == "Primary") == null)
            {
                var addressType = AddressType.NewActiveDraft(
                    "Primary"
                    , "Primary address"
                    , creatorUser
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.Now.AddYears(Consts.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES)
                );

                var result = await myDbContext.AddressTypes.AddAsync(addressType);

                await myUnitOfWork.SaveChangesAsync();
            }

            if (myDbContext.AddressTypes.SingleOrDefault(r => r.Name == "Secondary") == null)
            {
                var addressType = AddressType.NewActiveDraft(
                    "Secondary"
                    , "Secondary address"
                    , creatorUser
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.UtcNow
                    , DateTimeOffset.Now.AddYears(Consts.DEFAULT_ACTIVETO_VALUE_FOR_ADDRESSES)
                );

                var result = await myDbContext.AddressTypes.AddAsync(addressType);

                await myUnitOfWork.SaveChangesAsync();
            }

            if (myDbContext.AddressTypes.SingleOrDefault(r => r.Name == "Living") == null)
            {
                var addressType = AddressType.NewActiveDraft(
                    "Living"
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