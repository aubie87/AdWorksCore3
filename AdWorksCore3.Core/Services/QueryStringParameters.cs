using AdWorksCore3.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.Core.Services
{
    public abstract class QueryStringParameters
    {
        public readonly int MaxPageSize;
        public QueryStringParameters()
            : this(50)
        {
        }
        public QueryStringParameters(int maxPageSize)
        {
            MaxPageSize = maxPageSize;
            pageSize = MaxPageSize;
        }
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => this.pageSize;
            set => this.pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        protected int pageSize;

        public virtual IQueryable<Customer> Filter(IQueryable<Customer> query)
        {
            return query;
        }

        public virtual IQueryable<Customer> Search(IQueryable<Customer> query)
        {
            return query;
        }
        [FromQuery(Name = "search")]
        public string SearchQuery { get; set; }

    }
}
