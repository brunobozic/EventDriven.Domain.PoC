using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventDriven.Domain.PoC.Repository.EF.EntityConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // not for SQLite unfortunately
            //builder.Property(user => user.Id).UseHiLo("Sequence", "dbo");

            builder.HasMany(role => role.UserRoles)
                .WithOne(userRole => userRole.Role)
                .HasForeignKey(userRole => userRole.RoleId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(role => role.ReactivatedBy)
                .WithMany()
                .HasForeignKey(role => role.ReactivatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(role => role.ActivatedBy)
                .WithMany()
                .HasForeignKey(role => role.ActivatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(role => role.DeactivatedBy)
                .WithMany()
                .HasForeignKey(role => role.DeactivatedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(role => role.DeletedBy)
                .WithMany()
                .HasForeignKey(role => role.DeletedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(role => role.UndeletedBy)
                .WithMany()
                .HasForeignKey(role => role.UndeletedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(role => role.ModifiedBy)
                .WithMany()
                .HasForeignKey(role => role.ModifiedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasQueryFilter(p => !p.TheUserHasBeenDeleted);
        }
    }
}