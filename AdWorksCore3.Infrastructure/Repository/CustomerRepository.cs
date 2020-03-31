using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Interfaces;
using AdWorksCore3.Core.Services;
using AdWorksCore3.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace AdWorksCore3.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> logger;
        private readonly AdWorksContext context;

        public CustomerRepository(ILogger<CustomerRepository> logger, AdWorksContext context)
        {
            this.logger = logger;
            this.context = context;
        }
        public async Task<Customer> AddAsync(Customer customer)
        {
            logger.LogInformation("Adding new customer");
            try
            {
                context.Customer.Add(customer);
                await context.SaveChangesAsync();
                return customer;
            }
            catch (DbUpdateConcurrencyException dbuce)
            {
                logger.LogError(dbuce, "Saving changes for new customer failed.");
                throw dbuce;
            }
            catch (DbUpdateException dbue)
            {
                logger.LogError(dbue, "Saving changes for new customer failed.");
                throw dbue;
            }
        }

        public async Task<bool> Delete(int id)
        {
            // trick EF to think its tracking a deleted object - then save changes
            Customer customer = new Customer() { CustomerId = id };
            context.Entry(customer).State = EntityState.Deleted;
            try
            {
                int results = await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                // Probably does not exist.
                logger.LogWarning(e, $"Delete failed on Customer ID {id}");
            }
            return false;
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

        public async Task<IPagedList<Customer>> ListAsync(QueryStringParameters parameters)
        {
            var query = context.Customer
                .Include(c => c.CustomerAddress)
                .ThenInclude(ca => ca.Address) as IQueryable<Customer>;

            query = parameters.Filter(query);

            query = parameters.Search(query);

            query = query.OrderBy(c => c.CustomerId);

            var pagedList = await query.ToPagedListAsync(parameters.PageNumber, parameters.PageSize);
            return pagedList;
        }

        public async Task UpdateAsync(Customer customer)
        {
            context.Entry(customer).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
