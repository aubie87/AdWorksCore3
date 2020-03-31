using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.Web.ResourceParameters
{
    /// <summary>
    /// </summary>
    public class CustomersParameters : QueryStringParameters
    {
        private const int customerMaxPageSize = 25;
        public CustomersParameters()
            : base(customerMaxPageSize)
        {
        }

        public override IQueryable<Customer> Filter(IQueryable<Customer> query)
        {
            var collection = query;

            if(!string.IsNullOrWhiteSpace(LastName))
            {
                LastName.Trim();
                collection = collection.Where(c => c.LastName.Contains(LastName));
            }

            if(!string.IsNullOrWhiteSpace(Email))
            {
                Email.Trim();
                collection = collection.Where(c => c.EmailAddress.Contains(Email));
            }

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                Phone.Trim();
                collection = collection.Where(c => c.Phone.Contains(Phone));
            }

            //if (!string.IsNullOrWhiteSpace(StateProvince))
            //{
            //    // filter based on many to many sub-child with intermediate linking table via Ef Core?
            //    StateProvince.Trim();
            //    collection = collection.Where(c=>c.CustomerAddress.
            //}

            return collection;
        }

        public override IQueryable<Customer> Search(IQueryable<Customer> query)
        {
            if(string.IsNullOrWhiteSpace(SearchQuery))
            {
                return query;
            }
            SearchQuery.Trim();
            return query.Where(c=>c.CompanyName.Contains(SearchQuery)
                || c.FirstName.Contains(SearchQuery)
                || c.MiddleName.Contains(SearchQuery)
                || c.LastName.Contains(SearchQuery));
        }

        public string Email { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        //public string StateProvince { get; set; }
    }
}
