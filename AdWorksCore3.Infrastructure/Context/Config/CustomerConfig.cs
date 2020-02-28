using AdWorksCore3.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdWorksCore3.Infrastructure.Context.Config
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customer", "SalesLT");

            builder.HasComment("Customer information.");

            builder.HasIndex(e => e.EmailAddress);
            builder.HasIndex(e => e.Rowguid)
                .HasName("AK_Customer_rowguid")
                .IsUnique();

            builder.Property(e => e.CustomerId)
                .HasColumnName("CustomerID")
                .HasComment("Primary key for Customer records.");
            builder.Property(e => e.CompanyName)
                .HasMaxLength(128)
                .HasComment("The customer's organization.");
            builder.Property(e => e.EmailAddress)
                .HasMaxLength(50)
                .HasComment("E-mail address for the person.");
            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("First name of the person.");
            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Last name of the person.");
            builder.Property(e => e.MiddleName)
                .HasMaxLength(50)
                .HasComment("Middle name or middle initial of the person.");
            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.");
            builder.Property(e => e.NameStyle)
                .HasComment("0 = The data in FirstName and LastName are stored in western style (first name, last name) order.  1 = Eastern style (last name, first name) order.");
            builder.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasComment("Password for the e-mail account.");
            builder.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasComment("Random value concatenated with the password string before the password is hashed.");
            builder.Property(e => e.Phone)
                .HasMaxLength(25)
                .HasComment("Phone number associated with the person.");
            builder.Property(e => e.Rowguid)
                .HasColumnName("rowguid")
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");
            builder.Property(e => e.SalesPerson)
                .HasMaxLength(256)
                .HasComment("The customer's sales person, an employee of AdventureWorks Cycles.");
            builder.Property(e => e.Suffix)
                .HasMaxLength(10)
                .HasComment("Surname suffix. For example, Sr. or Jr.");
            builder.Property(e => e.Title)
                .HasMaxLength(8)
                .HasComment("A courtesy title. For example, Mr. or Ms.");
        }
    }
}
