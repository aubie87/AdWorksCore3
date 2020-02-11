using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AdWorksCore3.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration(SetupConfiguration)
                        .UseStartup<Startup>();
                });

        private static void SetupConfiguration(WebHostBuilderContext context, IConfigurationBuilder builder)
        {
            // Setup customer configuration loading here
            //builder.Sources.Clear();

            // should try to use logging pipeline - may not be configured at this point in loading
            Debug.WriteLine("Current Configuration Types:");
            foreach (var config in builder.Sources)
            {
                Debug.WriteLine($"   {config.GetType()}");
            }
        }
    }
}
