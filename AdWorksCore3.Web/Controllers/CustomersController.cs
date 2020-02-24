using AdWorksCore3.Core.Entities;
using AdWorksCore3.Web.Services;
using AdWorksCore3.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdWorksCore3.Core.Interfaces;
using System.Linq;

namespace AdWorksCore3.Web.Controllers
{
    [ApiVersion("2.0")]
    public class CustomersController : AdWorksControllerBase
    {
        private readonly ILogger<CustomersController> logger;
        private readonly ICustomerRepository repository;

        public CustomersController(ILogger<CustomersController> logger, ICustomerRepository context)
        {
            this.logger = logger;
            this.repository = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerGetViewModel>>> List()
        {
            logger.LogInformation("Getting list of customers using viewmodel class - approx last 100 only, reverse Id order");
            var customerList = await repository.ListAsync();
            return Ok(customerList.Select(c => CustomerGetViewModel.FromCustomerEntity(c)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerGetViewModel>> GetById(int id)
        {
            logger.LogInformation($"Get customer where id = {id}");
            Customer customer = await repository.GetByIdAsync(id);
            if(null == customer)
            {
                return NotFound(id);
            }
            CustomerGetViewModel vm = CustomerGetViewModel.FromCustomerEntity(customer);
            return Ok(vm);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerGetViewModel>> Create([FromBody] CustomerUpdateViewModel updateVm)
        {
            Customer customerDb = CustomerUpdateViewModel.ToCustomerEntity(updateVm);
            customerDb = PasswordService.GenerateNewHash(customerDb, customerDb.FirstName + customerDb.LastName);
            try
            {
                customerDb = await repository.AddAsync(customerDb);
                CustomerGetViewModel getVm = CustomerGetViewModel.FromCustomerEntity(customerDb);
                return CreatedAtAction(nameof(GetById), new { id = getVm.Id }, getVm);
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
            Customer customer = await repository.GetByIdAsync(id);
            if(customer == null)
            {
                // should PUT create the resource or error out?
                return NotFound();
            }

            customer = CustomerUpdateViewModel.ToCustomerEntity(customer, vm);
            await repository.UpdateAsync(customer);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if(!await repository.IdExistsAsync(id))
            {
                return NotFound("Unable to delete given key value.");
            }

            await repository.Delete(id);
            logger.LogInformation($"Deleted customer {id}");
            return NoContent();
        }
    }
}
