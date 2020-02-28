using AdWorksCore3.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdWorksCore3.Infrastructure.Context.Config
{
    public class ProductDescriptionConfig : IEntityTypeConfiguration<ProductDescription>
    {
        public void Configure(EntityTypeBuilder<ProductDescription> builder)
        {
            builder.ToTable("ProductDescription", "SalesLT");

            builder.HasComment("Product descriptions in several languages.");

            builder.HasIndex(e => e.Rowguid)
                .HasName("AK_ProductDescription_rowguid")
                .IsUnique();

            builder.Property(e => e.ProductDescriptionId)
                .HasColumnName("ProductDescriptionID")
                .HasComment("Primary key for ProductDescription records.");

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(400)
                .HasComment("Description of the product.");

            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.");

            builder.Property(e => e.Rowguid)
                .HasColumnName("rowguid")
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");
        }
    }
}
