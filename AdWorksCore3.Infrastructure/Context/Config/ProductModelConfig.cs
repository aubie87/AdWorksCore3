using AdWorksCore3.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdWorksCore3.Infrastructure.Context.Config
{
    public class ProductModelConfig : IEntityTypeConfiguration<ProductModel>
    {
        public void Configure(EntityTypeBuilder<ProductModel> builder)
        {
            builder.ToTable("ProductModel", "SalesLT");

            builder.HasIndex(e => e.CatalogDescription)
                .HasName("PXML_ProductModel_CatalogDescription");
            builder.HasIndex(e => e.Name)
                .HasName("AK_ProductModel_Name")
                .IsUnique();

            builder.Property(e => e.ProductModelId)
                .HasColumnName("ProductModelID");
            builder.Property(e => e.CatalogDescription)
                .HasColumnType("xml");
            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
