using AdWorksCore3.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.Web.ViewModels
{
    public class AddressGetViewModel
    {
        public int Id { get; set; }
        public string AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public DateTime ModifiedDate { get; set; }

        internal static AddressGetViewModel FromAddressEntity(CustomerAddress address)
        {
            return new AddressGetViewModel
            {
                Id = address.AddressId,
                AddressType = address.AddressType,
                AddressLine1 = address.Address.AddressLine1,
                AddressLine2 = address.Address.AddressLine2,
                City = address.Address.City,
                StateProvince = address.Address.StateProvince,
                PostalCode = address.Address.PostalCode,
                Country = address.Address.CountryRegion,
                ModifiedDate = address.Address.ModifiedDate
            };
        }
    }
}
