using AdWorksCore3.Core.Entities;
using AdWorksCore3.Web.Services;
using AdWorksCore3.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdWorksCore3.Core.Interfaces;
using System.Linq;
using System;
using AdWorksCore3.Web.ResourceParameters;
using AutoMapper;
using X.PagedList;
using AdWorksCore3.Core.Services;

namespace AdWorksCore3.Web.Controllers.Api
{
    [ApiVersion("2.0")]
    public class CustomersController : AdWorksControllerBase
    {
        private readonly ILogger<CustomersController> logger;
        private readonly IMapper mapper;
        private readonly ICustomerRepository repository;

        public CustomersController(ILogger<CustomersController> logger, IMapper mapper, ICustomerRepository context)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.repository = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerGetViewModel>> GetById(int id)
        {
            logger.LogInformation($"Get customer where id = {id}");
            Customer customer = await repository.GetByIdAsync(id);
            if (null == customer)
            {
                return NotFound(id);
            }

            CustomerGetViewModel vm = mapper.Map<CustomerGetViewModel>(customer);
            return Ok(vm);
        }

        [HttpGet(Name = "GetCustomers")]
        public async Task<ActionResult<IEnumerable<CustomerGetViewModel>>> GetPage(
            [FromQuery] CustomersParameters customerParameters)
        {
            var pagedList = await repository.ListAsync(customerParameters);
            var metaData = pagedList.GetMetaData();
            AddHeaders("GetCustomers", pagedList, customerParameters);

            return Ok(mapper.Map<IEnumerable<CustomerGetViewModel>>(pagedList));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerGetViewModel>> Create([FromBody] CustomerUpdateViewModel updateVm)
        {
            Customer customer = mapper.Map<Customer>(updateVm);
            customer = PasswordService.GenerateNewHash(customer, customer.FirstName + customer.LastName);
            try
            {
                customer = await repository.AddAsync(customer);
                CustomerGetViewModel getVm = mapper.Map<CustomerGetViewModel>(customer);
                return CreatedAtAction(nameof(GetById), new { id = getVm.Id }, getVm);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Saving changes for new customer failed.");
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

            mapper.Map(vm, customer);
            await repository.UpdateAsync(customer);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            logger.LogInformation($"Deleting customer {id}");

            if (await repository.Delete(id))
            {
                return NoContent();
            }
            else
            {
                return NotFound("Unable to delete given key value.");
            }
        }
    }
}
