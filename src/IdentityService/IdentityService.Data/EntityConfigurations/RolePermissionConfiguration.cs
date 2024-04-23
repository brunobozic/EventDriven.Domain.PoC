using IdentityService.Domain.DomainEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId, rp.ResourceId });

        builder.HasOne(rp => rp.Role)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.RoleId);

        builder.HasOne(rp => rp.Permission)
            .WithMany(p => p.RolePermissions)
            .HasForeignKey(rp => rp.PermissionId);

        builder.HasOne(rp => rp.Resource)
            .WithMany(r => r.RolePermissions)
            .HasForeignKey(rp => rp.ResourceId);

        // Relationship with ApplicationUser for DeletedBy
        builder.HasOne(d => d.DeletedBy)
            .WithMany() // Assuming ApplicationUser has no navigation property back to DepartmentCodebook; adjust as necessary
            .HasForeignKey(d => d.DeletedById)
            .OnDelete(DeleteBehavior.Restrict); // or another behavior as appropriate
    }
}