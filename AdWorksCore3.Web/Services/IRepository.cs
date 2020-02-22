using AdWorksCore3.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.Web.Services
{
    public interface IRepository
    {
        IEnumerable<Product> GetProducts();
    }
}
