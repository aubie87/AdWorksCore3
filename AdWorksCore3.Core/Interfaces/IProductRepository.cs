using AdWorksCore3.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdWorksCore3.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        Task<IEnumerable<Product>> ListAsync();
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task<bool> IdExistsAsync(int id);
        Task Delete(int id);
    }
}
