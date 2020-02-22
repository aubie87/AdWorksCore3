using AdWorksCore3.Data.Entities;
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

        public static Customer ToCustomerEntity(CustomerUpdateViewModel customerVm)
        {
            return new Customer()
            {
                FirstName = customerVm.FirstName,
                MiddleName = customerVm.MiddleName,
                LastName = customerVm.LastName,
                EmailAddress = customerVm.EmailAddress,
                Phone = customerVm.Phone,
                Suffix = customerVm.Suffix,
                Title = customerVm.Title,
                // required fields not supplied
                // NameStyle = false
            };
        }
    }
}