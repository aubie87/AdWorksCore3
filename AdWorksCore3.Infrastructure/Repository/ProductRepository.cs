using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Interfaces;
using AdWorksCore3.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AdWorksContext context;

        public ProductRepository(AdWorksContext context)
        {
            this.context = context;
        }
        public async Task<Product> AddAsync(Product product)
        {
            context.Product.Add(product);
            await context.SaveChangesAsync();
            return product;
        }

        public async Task Delete(int id)
        {
            // trick EF to think its tracking a deleted object - then save changes
            Product product = new Product() { ProductId = id };
            context.Entry(product).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await context.Product
                .Include(p => p.ProductModel)
                .FirstOrDefaultAsync(p=>p.ProductId == id);
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await context.Product.FindAsync(id) != null;
        }

        public async Task<IEnumerable<Product>> ListAsync()
        {
            return await context.Product
                .Include(p => p.ProductModel)
                .ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
