using IdentityService.Domain.DomainEntities.OutboxPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations;

internal sealed class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.ToTable("InternalCommands");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedNever();
    }
}