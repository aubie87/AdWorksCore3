using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Interfaces;
using AdWorksCore3.Infrastructure.Context;
using AdWorksCore3.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdWorksCore3.Cmd
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            //await serviceProvider.GetService<App>().RunAsync();

            // initialize a new instance that implements the interface
            ICustomerRepository repo = serviceProvider.GetService<ICustomerRepository>();
            Console.WriteLine($"Does 1 exist: {await repo.IdExistsAsync(1)}");
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            // required for AdWorksContext below - should be locally configured
            services.AddLogging();

            var config = LoadConfiguration();
            services.AddSingleton(config);
            services.AddTransient<App>();
            services.AddDbContext<AdWorksContext>(opt => opt
                .UseSqlServer(config.GetConnectionString("AdWorksContext"))
                .EnableSensitiveDataLogging(true));
            services.AddTransient<ICustomerRepository, CustomerRepository>();

            return services;
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
    }
}
