using AdWorksCore3.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdWorksCore3.Infrastructure.Context.Config
{
    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable("ProductCategory", "SalesLT");

            builder.HasComment("High-level product categorization.");

            builder.HasIndex(e => e.Name)
                .HasName("AK_ProductCategory_Name")
                .IsUnique();

            builder.Property(e => e.ProductCategoryId)
                .HasColumnName("ProductCategoryID")
                .HasComment("Primary key for ProductCategory records.");

            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Category description.");

            builder.Property(e => e.ParentProductCategoryId)
                .HasColumnName("ParentProductCategoryID")
                .HasComment("Product category identification number of immediate ancestor category. Foreign key to ProductCategory.ProductCategoryID.");

            builder.HasOne(d => d.ParentProductCategory)
                .WithMany(p => p.InverseParentProductCategory)
                .HasForeignKey(d => d.ParentProductCategoryId)
                .HasConstraintName("FK_ProductCategory_ProductCategory_ParentProductCategoryID_ProductCategoryID");
        }
    }
}
