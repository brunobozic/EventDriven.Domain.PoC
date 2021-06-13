using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EventDriven.Domain.PoC.Repository.EF.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            // not for SQLite unfortunately
            //builder.Property(user => user.Id).UseHiLo("Sequence", "dbo");

            builder.Property("_status").HasColumnName("StatusId")
                .HasConversion(new EnumToNumberConverter<RegistrationStatusEnum, byte>());

            builder.HasMany(user => user.RefreshTokens)
                .WithOne(refreshToken => refreshToken.ApplicationUser)
                .HasForeignKey(refreshToken => refreshToken.ApplicationUserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(user => user.JournalEntries)
                .WithOne(journalEntry => journalEntry.UserActedUpon)
                .HasForeignKey(journalEntry => journalEntry.JournalId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(user => user.UserRoles)
                .WithOne(userRole => userRole.User)
                .HasForeignKey(userRole => userRole.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(user => user.UserAddresses)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(user => user.ReactivatedBy)
                .WithMany()
                .HasForeignKey(user => user.ReactivatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(user => user.ActivatedBy)
                .WithMany()
                .HasForeignKey(user => user.ActivatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(user => user.DeactivatedBy)
                .WithMany()
                .HasForeignKey(user => user.DeactivatedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(user => user.DeletedBy)
                .WithMany()
                .HasForeignKey(user => user.DeletedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(user => user.UndeletedBy)
                .WithMany()
                .HasForeignKey(user => user.UndeletedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(user => user.ModifiedBy)
                .WithMany()
                .HasForeignKey(user => user.ModifiedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasQueryFilter(p => !p.Deleted);

            builder.UsePropertyAccessMode(PropertyAccessMode.Field);

            var navigation = builder.Metadata.FindNavigation(nameof(User.UserRoles));
            // DDD Patterns comment:
            // Set as field (New since EF 1.1) to access the UserRoles property through its field
            //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            var navigation2 = builder.Metadata.FindNavigation(nameof(User.RefreshTokens));

            // DDD Patterns comment:
            // Set as field (New since EF 1.1) to access the RefreshTokens collection property through its field
            //navigation2.SetPropertyAccessMode(PropertyAccessMode.Field);

            var navigation3 = builder.Metadata.FindNavigation(nameof(User.JournalEntries));

            // DDD Patterns comment:
            // Set as field (New since EF 1.1) to access the JournalEntries collection property through its field
            //navigation3.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}