using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace AdWorksCore3.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(int id);
        Task<IPagedList<Customer>> ListAsync(QueryStringParameters parameters);
        Task<Customer> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task<bool> IdExistsAsync(int id);
        Task<bool> Delete(int id);
    }
}
