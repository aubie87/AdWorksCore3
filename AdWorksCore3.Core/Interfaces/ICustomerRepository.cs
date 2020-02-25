using AdWorksCore3.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdWorksCore3.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> ListAsync();
        Task<Customer> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task<bool> IdExistsAsync(int id);
        Task Delete(int id);
    }
}
