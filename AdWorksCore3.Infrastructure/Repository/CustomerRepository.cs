using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Interfaces;
using AdWorksCore3.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AdWorksContext context;

        public CustomerRepository(AdWorksContext context)
        {
            this.context = context;
        }
        public async Task<Customer> AddAsync(Customer customer)
        {
            try
            {
                context.Customer.Add(customer);
                await context.SaveChangesAsync();
                return customer;
            }
            catch (DbUpdateConcurrencyException dbuce)
            {
                //logger.LogError(dbuce, "Saving changes for new customer failed.");
                Debug.WriteLine(dbuce, "Saving changes for new customer failed.");
                throw dbuce;
            }
            catch (DbUpdateException dbue)
            {
                Debug.WriteLine(dbue, "Saving changes for new customer failed.");
                throw dbue;
            }
        }

        public async Task Delete(int id)
        {
            // trick EF to think its tracking a deleted object - then save changes
            Customer customer = new Customer() { CustomerId = id };
            context.Entry(customer).State = EntityState.Deleted;
            await context.SaveChangesAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await context.Customer
                .Where(c => c.CustomerId == id)
                .Include(c => c.CustomerAddress)
                .ThenInclude(ca => ca.Address)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await context.Customer.FindAsync(id) != null;
        }

        public async Task<IEnumerable<Customer>> ListAsync()
        {
            return await context.Customer
                .Skip(700)
                .OrderByDescending(o => o.CustomerId)
                .Include(c => c.CustomerAddress)
                .ThenInclude(ca => ca.Address)
                .ToListAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            context.Entry(customer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
