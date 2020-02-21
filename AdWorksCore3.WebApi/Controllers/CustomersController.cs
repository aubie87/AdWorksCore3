﻿using AdWorksCore3.Data.Context;
using AdWorksCore3.Data.Entities;
using AdWorksCore3.WebApi.Services;
using AdWorksCore3.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.WebApi.Controllers
{
    [ApiVersion("2.0")]
    public class CustomersController : AdWorksControllerBase
    {
        private readonly ILogger<CustomersController> logger;
        private readonly AdWorksContext context;

        public CustomersController(ILogger<CustomersController> logger, AdWorksContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerGetViewModel>>> List()
        {
            logger.LogInformation("Getting list of customers using viewmodel class - approx last 100 only, reverse Id order");
            return await context.Customer
                .Skip(700)
                .OrderByDescending(o=>o.CustomerId)
                .Include(c=>c.CustomerAddress)
                .ThenInclude(ca=>ca.Address)
                .Select(c => CustomerGetViewModel.FromCustomerEntity(c)) 
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerGetViewModel>> GetById(int id)
        {
            logger.LogInformation($"Get customer where id = {id}");
            CustomerGetViewModel customer = await context.Customer
                .Where(c => c.CustomerId == id)
                .Include(c => c.CustomerAddress)
                .ThenInclude(ca => ca.Address)
                .Select(c => CustomerGetViewModel.FromCustomerEntity(c))
                .FirstOrDefaultAsync();

            if(null == customer)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerGetViewModel>> PostCustomer([FromBody] CustomerUpdateViewModel customerVm)
        {
            Customer customerDb = CustomerUpdateViewModel.ToCustomerEntity(customerVm);
            customerDb = PasswordService.GenerateNewHash(customerDb, customerDb.FirstName + customerDb.LastName);

            try
            {
                context.Customer.Add(customerDb);
                int result = await context.SaveChangesAsync();
                CustomerGetViewModel vm = CustomerGetViewModel.FromCustomerEntity(customerDb);
                return CreatedAtAction(nameof(GetById), new { id = vm.Id }, vm);
            }
            catch (DbUpdateConcurrencyException dbuce)
            {
                logger.LogError(dbuce, "Saving changes for new customer failed.");
            }
            catch (DbUpdateException dbue)
            {
                logger.LogError(dbue, "Saving changes for new customer failed.");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CustomerUpdateViewModel vm, int id)
        {
            Customer customerDb = await context.Customer.FindAsync(id);
            if(customerDb == null)
            {
                // should PUT create the resource or error out?
                return NotFound();
            }

            customerDb.FirstName = vm.FirstName;
            customerDb.MiddleName = vm.MiddleName;
            customerDb.LastName = vm.LastName;
            customerDb.EmailAddress = vm.EmailAddress;
            customerDb.Phone = vm.Phone;
            customerDb.Suffix = vm.Suffix;
            customerDb.Title = vm.Title;
            context.Entry(customerDb).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Customer customer = await context.Customer.FindAsync(id);
            if(customer == null)
            {
                return NotFound("Unable to delete give key value.");
            }

            context.Customer.Remove(customer);
            int result = await context.SaveChangesAsync();
            logger.LogInformation($"SaveChanges={result}");
            return NoContent();
        }
    }
}
