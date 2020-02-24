using AdWorksCore3.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdWorksCore3.Web
{
    public class CustomerUpdateViewModel
    {
        public string Title { get; set; }
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        public string Suffix { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Phone { get; set; }

        /// <summary>
        /// Given the customer and a view model, update and return the customer.
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static Customer ToCustomerEntity(Customer customer, CustomerUpdateViewModel vm)
        {
            customer.FirstName = vm.FirstName;
            customer.MiddleName = vm.MiddleName;
            customer.LastName = vm.LastName;
            customer.EmailAddress = vm.EmailAddress;
            customer.Phone = vm.Phone;
            customer.Suffix = vm.Suffix;
            customer.Title = vm.Title;
            // required fields not supplied
            // NameStyle = false
            return customer;
        }

        /// <summary>
        /// Return a new Customer based on the given view model.
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public static Customer ToCustomerEntity(CustomerUpdateViewModel vm)
        {
            return new Customer()
            {
                MiddleName = vm.MiddleName,
                LastName = vm.LastName,
                FirstName = vm.FirstName,
                EmailAddress = vm.EmailAddress,
                Phone = vm.Phone,
                Suffix = vm.Suffix,
                Title = vm.Title,
                // required fields not supplied
                // NameStyle = false
            };
        }
        public static CustomerUpdateViewModel FromCustomerEntity(Customer customer)
        {
            return new CustomerUpdateViewModel()
            {
                Title = customer.Title,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                Suffix = customer.Suffix,
                EmailAddress = customer.EmailAddress,
                Phone = customer.Phone
            };
        }
    }
}