using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityService.Data.EntityConfigurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        // not for SQLite unfortunately
        //builder.Property(user => user.Id).UseHiLo("Sequence", "dbo");

        builder
            .Property<int?>("_addressTypeId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("AddressTypeId                                   ")
            .IsRequired(false);
        builder
            .Property<int?>("_cityBlockId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CityBlockId")
            .IsRequired(false);
        builder
            .Property<int?>("_countyId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CountyId")
            .IsRequired(false);
        builder
            .Property<int?>("_countryId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("CountryId")
            .IsRequired(false);
        builder
            .Property<int?>("_townId")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("TownId")
            .IsRequired(false);

        builder.Property(address => address.Line1)
            .IsRequired();

        builder.Property(address => address.Line2)
            .IsRequired(false);

        builder.Property(address => address.FlatNr)
            .IsRequired(false);

        builder.Property(address => address.HouseNumber)
            .IsRequired();

        builder.Property(address => address.HouseNumberSuffix)
            .IsRequired(false);

        builder.Property(address => address.UserComment)
            .IsRequired(false);

        builder.HasMany(address => address.UserAddresses)
            .WithOne(userAddress => userAddress.Address)
            .HasForeignKey(userAddress => userAddress.AddressId)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasOne(address => address.ReactivatedBy)
            .WithMany()
            .HasForeignKey(address => address.ReactivatedById)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false)
            ;

        builder.HasOne(address => address.ActivatedBy)
            .WithMany()
            .HasForeignKey(address => address.ActivatedById)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false)
            ;

        builder.HasOne(address => address.DeactivatedBy)
            .WithMany()
            .HasForeignKey(address => address.DeactivatedById)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasOne(address => address.DeletedBy)
            .WithMany()
            .HasForeignKey(address => address.DeletedById)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasOne(address => address.UndeletedBy)
            .WithMany()
            .HasForeignKey(address => address.UndeletedById)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasOne(address => address.ModifiedBy)
            .WithMany()
            .HasForeignKey(address => address.ModifiedById)
            .OnDelete(DeleteBehavior.SetNull)
            ;

        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}