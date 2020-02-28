using AdWorksCore3.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdWorksCore3.Infrastructure.Context.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product", "SalesLT");

            builder.HasComment("Products sold or used in the manfacturing of sold products.");

            builder.HasIndex(e => e.Name)
                .HasName("AK_Product_Name")
                .IsUnique();
            builder.HasIndex(e => e.ProductNumber)
                .HasName("AK_Product_ProductNumber")
                .IsUnique();
            builder.HasIndex(e => e.Rowguid)
                .HasName("AK_Product_rowguid")
                .IsUnique();

            builder.Property(e => e.ProductId)
                .HasColumnName("ProductID")
                .HasComment("Primary key for Product records.");
            builder.Property(e => e.Color)
                .HasMaxLength(15)
                .HasComment("Product color.");
            builder.Property(e => e.DiscontinuedDate)
                .HasColumnType("datetime")
                .HasComment("Date the product was discontinued.");
            builder.Property(e => e.ListPrice)
                .HasColumnType("money")
                .HasComment("Selling price.");
            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date and time the record was last updated.");
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasComment("Name of the product.");
            builder.Property(e => e.ProductCategoryId)
                .HasColumnName("ProductCategoryID")
                .HasComment("Product is a member of this product category. Foreign key to ProductCategory.ProductCategoryID. ");
            builder.Property(e => e.ProductModelId)
                .HasColumnName("ProductModelID")
                .HasComment("Product is a member of this product model. Foreign key to ProductModel.ProductModelID.");
            builder.Property(e => e.ProductNumber)
                .IsRequired()
                .HasMaxLength(25)
                .HasComment("Unique product identification number.");
            builder.Property(e => e.Rowguid)
                .HasColumnName("rowguid")
                .HasDefaultValueSql("(newid())")
                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");
            builder.Property(e => e.SellEndDate)
                .HasColumnType("datetime")
                .HasComment("Date the product was no longer available for sale.");
            builder.Property(e => e.SellStartDate)
                .HasColumnType("datetime")
                .HasComment("Date the product was available for sale.");
            builder.Property(e => e.Size)
                .HasMaxLength(5)
                .HasComment("Product size.");
            builder.Property(e => e.StandardCost)
                .HasColumnType("money")
                .HasComment("Standard cost of the product.");
            builder.Property(e => e.ThumbNailPhoto)
                .HasComment("Small image of the product.");
            builder.Property(e => e.ThumbnailPhotoFileName)
                .HasMaxLength(50)
                .HasComment("Small image file name.");
            builder.Property(e => e.Weight)
                .HasColumnType("decimal(8, 2)")
                .HasComment("Product weight.");

            builder.HasOne(d => d.ProductCategory)
                .WithMany(p => p.Product)
                .HasForeignKey(d => d.ProductCategoryId);
            builder.HasOne(d => d.ProductModel)
                .WithMany(p => p.Product)
                .HasForeignKey(d => d.ProductModelId);
        }
    }
}
