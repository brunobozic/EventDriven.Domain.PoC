using IdentityService.Domain.DomainEntities.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations;

internal sealed class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessage>
{
    public void Configure(EntityTypeBuilder<InboxMessage> builder)
    {
        builder.ToTable("InboxMessages");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedNever();
    }
}