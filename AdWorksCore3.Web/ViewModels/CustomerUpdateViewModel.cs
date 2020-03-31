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
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }


        public static CustomerUpdateViewModel FromCustomerEntity(Customer customer)
        {
            return new CustomerUpdateViewModel()
            {
                Title = customer.Title,
                FirstName = customer.FirstName,
                MiddleName = customer.MiddleName,
                LastName = customer.LastName,
                Suffix = customer.Suffix,
                Email = customer.EmailAddress,
                Phone = customer.Phone
            };
        }
    }
}