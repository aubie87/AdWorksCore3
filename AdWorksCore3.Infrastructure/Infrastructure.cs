using AdWorksCore3.Core.Interfaces;
using AdWorksCore3.Infrastructure.Context;
using AdWorksCore3.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.Infrastructure
{
    public class Infrastructure
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<AdWorksContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("AdWorksContext"))
                   .EnableSensitiveDataLogging());

            // provides a factory method to implement a custom instance
            //services.AddScoped<IRepository>(GetRepository);
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
