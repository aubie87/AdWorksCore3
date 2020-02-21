using AdWorksCore3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.WebApi.Services
{
    public interface IRepository
    {
        IEnumerable<Product> GetProducts();
    }
}
