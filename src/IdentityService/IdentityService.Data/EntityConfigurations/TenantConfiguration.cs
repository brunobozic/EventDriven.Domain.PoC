using IdentityService.Domain.DomainEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(e => e.Name).HasColumnName("TenantName").HasMaxLength(75).IsRequired();

    }
}