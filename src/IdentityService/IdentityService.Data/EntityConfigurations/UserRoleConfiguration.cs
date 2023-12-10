using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(ur => ur.Id);

        // not for SQLite unfortunately
        //builder.Property(user => user.Id).UseHiLo("Sequence", "dbo");

        builder.HasOne(userRole => userRole.User)
            .WithMany(user => user.UserRoles)
            .HasForeignKey(userRole => userRole.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(userRole => userRole.Role)
            .WithMany(role => role.UserRoles)
            .HasForeignKey(userRole => userRole.RoleId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(userRole => userRole.ReactivatedBy)
            .WithMany()
            .HasForeignKey(userRole => userRole.ReactivatedById)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasOne(userRole => userRole.ActivatedBy)
            .WithMany()
            .HasForeignKey(userRole => userRole.ActivatedById)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasOne(userRole => userRole.DeactivatedBy)
            .WithMany()
            .HasForeignKey(userRole => userRole.DeactivatedById)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasOne(userRole => userRole.DeletedBy)
            .WithMany()
            .HasForeignKey(userRole => userRole.DeletedById)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasOne(userRole => userRole.UndeletedBy)
            .WithMany()
            .HasForeignKey(userRole => userRole.UndeletedById)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasOne(userRole => userRole.ModifiedBy)
            .WithMany()
            .HasForeignKey(userRole => userRole.ModifiedById)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}