using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Interfaces;
using AdWorksCore3.Web.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AdWorksCore3.Web.Pages.Razor.Customers
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Customer> CustomerList { get; private set; }

        private readonly ICustomerRepository repository;
        private readonly ILogger<IndexModel> logger;

        public IndexModel(ICustomerRepository repository, ILogger<IndexModel> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        public async Task OnGet(CustomersParameters parameters)
        {
            CustomerList = await repository.ListAsync(parameters);
            logger.LogInformation($"CustomerList count={CustomerList.Count()}");
        }
    }
}