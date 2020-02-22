using AdWorksCore3.Infrastructure.Context;
using AdWorksCore3.Infrastructure.Entities;
using AdWorksCore3.Web.Services;
using AdWorksCore3.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.Web.Controllers
{
    [ApiVersion("2.0")]
    public class CustomersController : AdWorksControllerBase
    {
        private readonly ILogger<CustomersController> logger;
        private readonly ICustomerRepository context;

        public CustomersController(ILogger<CustomersController> logger, ICustomerRepository context)
        {
            this.logger = logger;
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerGetViewModel>>> List()
        {
            logger.LogInformation("Getting list of customers using viewmodel class - approx last 100 only, reverse Id order");
            //return await context.Customer
            //    .Skip(700)
            //    .OrderByDescending(o=>o.CustomerId)
            //    .Include(c=>c.CustomerAddress)
            //    .ThenInclude(ca=>ca.Address)
            //    .Select(c => CustomerGetViewModel.FromCustomerEntity(c)) 
            //    .ToListAsync();
            return Ok(await context.ListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerGetViewModel>> GetById(int id)
        {
            logger.LogInformation($"Get customer where id = {id}");
            //CustomerGetViewModel customer = await context.Customer
            //    .Where(c => c.CustomerId == id)
            //    .Include(c => c.CustomerAddress)
            //    .ThenInclude(ca => ca.Address)
            //    .Select(c => CustomerGetViewModel.FromCustomerEntity(c))
            //    .FirstOrDefaultAsync();
            CustomerGetViewModel customer = await context.GetByIdAsync(id);

            if(null == customer)
            {
                return NotFound(id);
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerGetViewModel>> Create([FromBody] CustomerUpdateViewModel customerVm)
        {
            Customer customerDb = CustomerUpdateViewModel.ToCustomerEntity(customerVm);
            customerDb = PasswordService.GenerateNewHash(customerDb, customerDb.FirstName + customerDb.LastName);

            try
            {
                //context.Customer.Add(customerDb);
                //int result = await context.SaveChangesAsync();
                //CustomerGetViewModel vm = CustomerGetViewModel.FromCustomerEntity(customerDb);
                CustomerGetViewModel vm = await context.AddAsync(customerVm);
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
            //Customer customerDb = await context.Customer.FindAsync(id);
            //if(customerDb == null)
            if(!await context.IdExistsAsync(id))
            {
                // should PUT create the resource or error out?
                return NotFound();
            }

            await context.UpdateAsync(vm);

            //customerDb.FirstName = vm.FirstName;
            //customerDb.MiddleName = vm.MiddleName;
            //customerDb.LastName = vm.LastName;
            //customerDb.EmailAddress = vm.EmailAddress;
            //customerDb.Phone = vm.Phone;
            //customerDb.Suffix = vm.Suffix;
            //customerDb.Title = vm.Title;
            //context.Entry(customerDb).State = EntityState.Modified;
            //await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            //Customer customer = await context.Customer.FindAsync(id);
            //if(customer == null)
            if(!await context.IdExistsAsync(id))
            {
                return NotFound("Unable to delete give key value.");
            }

            //context.Customer.Remove(customer);
            //int result = await context.SaveChangesAsync();
            await context.Delete(id);
            logger.LogInformation($"Deleted customer {id}");
            return NoContent();
        }
    }
}
