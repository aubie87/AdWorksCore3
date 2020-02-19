using AdWorksCore3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdWorksCore3.WebApi.ViewModels
{
    public class CustomerGetViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public DateTime LastModified { get; set; }
        public ICollection<AddressGetViewModel> Addresses { get; set; } = new List<AddressGetViewModel>();

        /// <summary>
        /// Until we add an auto-mapping library we will use this style.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        internal static CustomerGetViewModel FromCustomerEntity(Customer customer)
        {
            return new CustomerGetViewModel
            {
                Id = customer.CustomerId,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                LastModified = customer.ModifiedDate,
                Phone = customer.Phone,
                Suffix = customer.Suffix,
                Title = customer.Title,
                Addresses = customer.CustomerAddress.Select(a => AddressGetViewModel.FromAddressEntity(a)).ToList()
            };
        }
    }
}
