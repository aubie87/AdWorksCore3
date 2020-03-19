using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AdWorksCore3.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration config;
        private readonly ILogger<IndexModel> logger;

        public IndexModel(IConfiguration config, ILogger<IndexModel> logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public string DefaultLogLevel { get; private set; }

        public void OnGet()
        {
            logger.LogInformation("Called OnGet(): " + HttpContext.TraceIdentifier);

            DefaultLogLevel = config["Logging:LogLevel:Default"];
        }
    }
}