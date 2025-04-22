using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations;

public class CountryConfiguration : IEntityTypeConfiguration<CountryCodebook>
{
    public void Configure(EntityTypeBuilder<CountryCodebook> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(e => e.Name).HasColumnName("INT_NAME").HasMaxLength(75).IsRequired();
        builder.Property(e => e.LocalName).HasColumnName("HR_NAME").HasMaxLength(75).IsRequired();
        builder.Property(e => e.ISO_3166_ALPHA_2).HasColumnName("ISO_3166_ALPHA_2").HasMaxLength(2);
        builder.Property(e => e.ISO_3166_ALPHA_3).HasColumnName("ISO_3166_ALPHA_3").HasMaxLength(3);
        builder.Property(e => e.ISO_3166_NUMERIC).HasColumnName("ISO_3166_NUMERIC").HasMaxLength(3);
    }
}