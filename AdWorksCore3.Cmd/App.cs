using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Interfaces;
using AdWorksCore3.Infrastructure.Context;
using AdWorksCore3.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdWorksCore3.Cmd
{
    public class App
    {
        private readonly IConfiguration config;
        private readonly ICustomerRepository repository;

        public App(IConfiguration config, ICustomerRepository repository)
        {
            this.config = config;
            this.repository = repository;
        }
        public async Task RunAsync()
        {
            Console.WriteLine(config.GetConnectionString("AdWorksContext"));
            //await Task.Delay(20);

            //await ListCustomers();
            //await ShowCustomer(35121);
            await UpdateCustomer(35121);
        }
        private async Task ListCustomers()
        {
            var customerList = await repository.ListAsync();
            int count = 0;
            foreach (var customer in customerList)
            {
                LogCustomer(customer);
                count++;
            }
            Console.WriteLine($"There are {count} active customers");
        }
        private async Task UpdateCustomer(int id)
        {
            Customer customer = await repository.GetByIdAsync(id);
            LogCustomer(customer);
            customer.Phone = Guid.NewGuid().ToString().Substring(5, 12);
            await repository.UpdateAsync(customer);
            LogCustomer(customer);
        }
        private async Task ShowCustomer(int id)
        {
            Customer customer = await repository.GetByIdAsync(id);
            LogCustomer(customer);
        }
        private void LogCustomer(Customer customer)
        {
            Console.WriteLine($"{customer.CustomerId}: {customer.Title} {customer.FirstName} {customer.LastName}");
            Console.WriteLine($"    {customer.EmailAddress} {customer.Phone}");
        }
    }
}
