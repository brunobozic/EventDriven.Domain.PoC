using IdentityService.Domain.DomainEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(o => o.Id);

        // Assuming ApplicationUser is properly configured somewhere else in your model
        // Relationship with ApplicationUser for ActivatedBy
        builder.HasOne(d => d.ActivatedBy)
            .WithMany()
            .HasForeignKey(d => d.ActivatedByApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust the DeleteBehavior as necessary

        // Relationship with ApplicationUser for CreatedBy
        builder.HasOne(d => d.CreatedBy)
            .WithMany()
            .HasForeignKey(d => d.CreatedByApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust the DeleteBehavior as necessary

        // Relationship with ApplicationUser for DeletedBy
        builder.HasOne(d => d.DeletedBy)
            .WithMany()
            .HasForeignKey(d => d.DeletedByApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust the DeleteBehavior as necessary

        // Relationship with ApplicationUser for ModifiedBy
        builder.HasOne(d => d.ModifiedBy)
            .WithMany()
            .HasForeignKey(d => d.ModifiedByApplicationUserId)
            .OnDelete(DeleteBehavior.Restrict); // Adjust the DeleteBehavior as necessary

        // Additional configuration for the Resource entity...
    }
}