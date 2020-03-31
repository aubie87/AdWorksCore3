using AdWorksCore3.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdWorksCore3.Infrastructure.Context.Config
{
    public class CustomerAddressConfig : IEntityTypeConfiguration<CustomerAddress>
    {
        public void Configure(EntityTypeBuilder<CustomerAddress> builder)
        {
            builder.HasKey(e => new { e.CustomerId, e.AddressId })
                .HasName("PK_CustomerAddress_CustomerID_AddressID");

            builder.ToTable("CustomerAddress", "SalesLT");

            builder.HasComment("Cross-reference table mapping customers to their address(es).");

            builder.Property(e => e.CustomerId)
                .HasColumnName("CustomerID")
                .HasComment("Primary key. Foreign key to Customer.CustomerID.");
            builder.Property(e => e.AddressId)
                .HasColumnName("AddressID")
                .HasComment("Primary key. Foreign key to Address.AddressID.");
            builder.Property(e => e.AddressType)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("The kind of Address. One of: Archive, Billing, Home, Main Office, Primary, Shipping");
            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.");

            builder.HasOne(d => d.Address)
                .WithMany(p => p.CustomerAddress)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(d => d.Customer)
                .WithMany(p => p.CustomerAddress)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
