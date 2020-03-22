using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace AdWorksCore3.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
               opt.ReturnHttpNotAcceptable = true;      // explicity returns an error when requesting an unsupported media type
            });
                // .AddXmlDataContractSerializerFormatters();  // adds XML result support but exposes view model name

            services.AddRazorPages();
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });
            services.AddRouting(opt =>
            {
                opt.LowercaseUrls = true;
                opt.LowercaseQueryStrings = true;
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            Infrastructure.Infrastructure.ConfigureServices(Configuration, services);
        }

        //private IRepository GetRepository(IServiceProvider provider)
        //{
        //    return provider.GetService();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                
                // redirects browsers to secure connection 
                //  - WARNING: Web API clients generally don't support automatic redirection
                //  - need to either not listen unsecurely or respond with 400 - Bad Request
                //app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();

            // must appear between UseRouting above and UseEndPoints below
            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
