using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.Web.ResourceParameters
{
    /// <summary>
    /// This class should be moved to Core since the repository
    /// needs to be aware of this functionality/properties.
    /// </summary>
    public class CustomersParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => this.pageSize;
            set => this.pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        const int maxPageSize = 50;
        private int pageSize;
    }
}
