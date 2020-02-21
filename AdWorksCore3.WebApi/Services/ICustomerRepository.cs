using AdWorksCore3.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.WebApi.Services
{
    public interface ICustomerRepository
    {
        Task<CustomerGetViewModel> GetByIdAsync(int id);
        Task<List<CustomerGetViewModel>> ListAsync();
        Task<CustomerGetViewModel> AddAsync(CustomerUpdateViewModel customer);
        Task UpdateAsync(CustomerUpdateViewModel customer);
        Task<bool> IdExistsAsync(int id);
        Task Delete(int id);
    }
}
