using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventDriven.Domain.PoC.Repository.EF.EntityConfigurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasKey(userAddress => userAddress.Id);

            // not for SQLite unfortunately
            //builder.Property(user => user.Id).UseHiLo("Sequence", "dbo");

            builder.HasOne(userAddress => userAddress.User)
                .WithMany(e => e.UserAddresses)
                .HasForeignKey(userAddress => userAddress.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(userAddress => userAddress.Address)
                .WithMany(e => e.UserAddresses)
                .HasForeignKey(userAddress => userAddress.AddressId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(userAddress => userAddress.ReactivatedBy)
                .WithMany()
                .HasForeignKey(userAddress => userAddress.ReactivatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(userAddress => userAddress.ActivatedBy)
                .WithMany()
                .HasForeignKey(userAddress => userAddress.ActivatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(userAddress => userAddress.DeactivatedBy)
                .WithMany()
                .HasForeignKey(userAddress => userAddress.DeactivatedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(userAddress => userAddress.DeletedBy)
                .WithMany()
                .HasForeignKey(userAddress => userAddress.DeletedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(userAddress => userAddress.UndeletedBy)
                .WithMany()
                .HasForeignKey(userAddress => userAddress.UndeletedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(userAddress => userAddress.ModifiedBy)
                .WithMany()
                .HasForeignKey(userAddress => userAddress.ModifiedById)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasOne(userAddress => userAddress.CreatedBy)
                .WithMany()
                .HasForeignKey(userAddress => userAddress.CreatedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull)
                ;

            builder.HasQueryFilter(p => !p.IsDeleted());
        }
    }
}