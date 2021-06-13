using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventDriven.Domain.PoC.Repository.EF.EntityConfigurations
{
    public class AddressTypeConfiguration : IEntityTypeConfiguration<AddressType>
    {
        public void Configure(EntityTypeBuilder<AddressType> builder)
        {
            // not for SQLite unfortunately
            //builder.Property(user => user.Id).UseHiLo("Sequence", "dbo");

            //builder.HasMany(addressType => addressType.Addresses)
            //    .WithOne(address => address.AddressType)
            //    .HasForeignKey(address => address.AddressTypeId)
            //    .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(addressType => addressType.ReactivatedBy)
                .WithMany()
                .HasForeignKey(addressType => addressType.ReactivatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(addressType => addressType.ActivatedBy)
                .WithMany()
                .HasForeignKey(addressType => addressType.ActivatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(addressType => addressType.DeactivatedBy)
                .WithMany()
                .HasForeignKey(addressType => addressType.DeactivatedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(addressType => addressType.DeletedBy)
                .WithMany()
                .HasForeignKey(addressType => addressType.DeletedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(addressType => addressType.UndeletedBy)
                .WithMany()
                .HasForeignKey(addressType => addressType.UndeletedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(addressType => addressType.ModifiedBy)
                .WithMany()
                .HasForeignKey(addressType => addressType.ModifiedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;
        }
    }
}