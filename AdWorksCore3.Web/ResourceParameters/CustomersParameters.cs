using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Services;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

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

        public override object GetPrevPageQueryString()
        {
            return new
            {
                pageNumber = PageNumber - 1,
                PageSize,
                search = SearchQuery,
                Email,
                LastName,
                Phone
            };
        }
        public override object GetNextPageQueryString()
        {
            return new
            {
                pageNumber = PageNumber + 1,
                PageSize,
                search = SearchQuery,
                Email,
                LastName,
                Phone
            };
        }

        public override IQueryable<Customer> OrderBy(IQueryable<Customer> query)
        {
            if (string.IsNullOrWhiteSpace(OrderByQuery))
            {
                // return the default order
                return query.OrderBy(c => c.CustomerId);
            }

            var orderByBuilder = new StringBuilder();

            // ex. "LastName, email desc"
            var orderParams = OrderByQuery.Trim().ToLower().Split(',');
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param)) continue;

                var paramName = param.Trim().Split(' ').First();
                var objProperty = "";
                var orderByDirection = "";

                switch (paramName)
                {
                    case "id":
                        objProperty = "CustomerId";
                        orderByDirection = GetOrderByDirection(param);
                        break;

                    case "email":
                        objProperty = "EmailAddress";
                        orderByDirection = GetOrderByDirection(param);
                        break;

                    case "lastmodified":
                        objProperty = "ModifiedDate";
                        orderByDirection = GetOrderByDirection(param);
                        break;

                    case "lastname":
                    case "companyname":
                        objProperty = paramName;
                        orderByDirection = GetOrderByDirection(param);
                        break;

                    default:
                        break;
                }
                if (orderByBuilder.Length > 0) orderByBuilder.Append(", ");
                orderByBuilder.AppendFormat($"{objProperty} {orderByDirection}");
            }

            // nuget package: System.Linq.Dynamic.Core
            //  provides string based query in OrderBy clause
            var orderByQuery = orderByBuilder.ToString();
            return query.OrderBy(orderByQuery);
        }

        private string GetOrderByDirection(string param)
        {
            if (param.EndsWith(" desc")) return "desc";
            if(param.EndsWith(" asc")) return "asc";
            return string.Empty;
        }
    }
}
