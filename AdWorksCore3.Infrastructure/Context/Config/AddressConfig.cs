using AdWorksCore3.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdWorksCore3.Infrastructure.Context.Config
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address", "SalesLT");
            builder.HasComment("Street address information for customers.");
            builder.HasIndex(e => e.StateProvince);
            builder.HasIndex(e => new { e.AddressLine1, e.AddressLine2, e.City, e.StateProvince, e.PostalCode, e.CountryRegion });
            builder.Property(e => e.AddressId)
                .HasColumnName("AddressID")
                .HasComment("Primary key for Address records.");

            builder.Property(e => e.AddressLine1)
                .IsRequired()
                .HasMaxLength(60)
                .HasComment("First street address line.");

            builder.Property(e => e.AddressLine2)
                .HasMaxLength(60)
                .HasComment("Second street address line.");

            builder.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(30)
                .HasComment("Name of the city.");

            builder.Property(e => e.CountryRegion)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.");

            builder.Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(15)
                .HasComment("Postal code for the street address.");

            builder.Property(e => e.StateProvince)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Name of state or province.");
        }
    }
}
