using AdWorksCore3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.WebApi.ViewModels
{
    public class ProductGetViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public string Color { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string Size { get; set; }
        public decimal? Weight { get; set; }
        //public int? ProductCategoryId { get; set; }
        //public int? ProductModelId { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public static ProductGetViewModel FromProductEntity(Product p)
        {
            return new ProductGetViewModel
            {
                Id = p.ProductId,
                Color = p.Color,
                DiscontinuedDate = p.DiscontinuedDate,
                ListPrice = p.ListPrice,
                ModifiedDate = p.ModifiedDate,
                Name = p.Name,
                ProductNumber = p.ProductNumber,
                SellEndDate = p.SellEndDate,
                SellStartDate = p.SellStartDate,
                Size = p.Size,
                StandardCost = p.StandardCost,
                Weight = p.Weight
            };
        }
    }
}
