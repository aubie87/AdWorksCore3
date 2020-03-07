using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdWorksCore3.Core.Entities;
using Microsoft.Extensions.Logging;
using AdWorksCore3.Web.ViewModels;
using AdWorksCore3.Core.Interfaces;
using System;

namespace AdWorksCore3.Web.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<CustomersController> logger;
        private readonly IProductRepository repository;

        public ProductsController(ILogger<CustomersController> logger, IProductRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductGetViewModel>>> GetProduct()
        {
            var productList = await repository.ListAsync();
            return Ok(productList.Select(p => ProductGetViewModel.FromProductEntity(p)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductGetViewModel>> GetProduct(int id)
        {
            var product = await repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return ProductGetViewModel.FromProductEntity(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            try
            {
                await repository.UpdateAsync(product);
            }
            catch (Exception e)
            {
                if (!await ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw e;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            await repository.AddAsync(product);

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            if (!await repository.IdExistsAsync(id))
            {
                return NotFound();
            }

            await repository.Delete(id);

            return NoContent();
        }

        private async Task<bool> ProductExists(int id)
        {
            return await repository.IdExistsAsync(id);
        }
    }
}
