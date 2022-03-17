using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AccountJournal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace EventDriven.Domain.PoC.Repository.EF.EntityConfigurations
{
    public class AccountJournalEntryConfiguration : IEntityTypeConfiguration<AccountJournalEntry>
    {
        public void Configure(EntityTypeBuilder<AccountJournalEntry> builder)
        {
            builder.HasKey(p => p.JournalId);
            builder.Property(p => p.JournalId)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Message)
                .IsRequired();

            builder.Property(p => p.UserNameActedUpon)
                .IsRequired();

            builder.Property(p => p.EmailActedUpon)
                .IsRequired();

            builder.Property(p => p.ActingUserName)
                .IsRequired(false);

            builder.Property(p => p.ActingEmail)
                .IsRequired(false);

            builder.Property(p => p.Seen)
                .IsRequired(false);

            builder.Property(p => p.DateCreated)
                .IsRequired()
                .ValueGeneratedOnAdd()
                .HasDefaultValue(DateTimeOffset.UtcNow);

            builder.Property(p => p.DateDeleted)
                .IsRequired(false);

            builder.HasOne(e => e.UserActedUpon)
                .WithMany(e => e.JournalEntries)
                .HasForeignKey(e => e.JournalId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.ActingUser)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            //// DDD Patterns comment:
            ////Set as field (New since EF 1.1) to access the User property through its field
            builder.UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}