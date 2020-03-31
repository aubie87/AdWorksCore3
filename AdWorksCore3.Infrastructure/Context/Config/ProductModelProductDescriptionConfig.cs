using AdWorksCore3.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdWorksCore3.Infrastructure.Context.Config
{
    public class ProductModelProductDescriptionConfig : IEntityTypeConfiguration<ProductModelProductDescription>
    {
        public void Configure(EntityTypeBuilder<ProductModelProductDescription> builder)
        {
            builder.HasKey(e => new { e.ProductModelId, e.ProductDescriptionId, e.Culture })
                .HasName("PK_ProductModelProductDescription_ProductModelID_ProductDescriptionID_Culture");

            builder.ToTable("ProductModelProductDescription", "SalesLT");

            builder.HasComment("Cross-reference table mapping product descriptions and the language the description is written in.");

            builder.Property(e => e.ProductModelId)
                .HasColumnName("ProductModelID")
                .HasComment("Primary key. Foreign key to ProductModel.ProductModelID.");
            builder.Property(e => e.ProductDescriptionId)
                .HasColumnName("ProductDescriptionID")
                .HasComment("Primary key. Foreign key to ProductDescription.ProductDescriptionID.");
            builder.Property(e => e.Culture)
                .HasMaxLength(6)
                .IsFixedLength()
                .HasComment("The culture for which the description is written");
            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.");

            builder.HasOne(d => d.ProductDescription)
                .WithMany(p => p.ProductModelProductDescription)
                .HasForeignKey(d => d.ProductDescriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(d => d.ProductModel)
                .WithMany(p => p.ProductModelProductDescription)
                .HasForeignKey(d => d.ProductModelId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
