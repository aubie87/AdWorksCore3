﻿using System;
using System.Collections.Generic;

namespace AdWorksCore3.Web.ViewModels
{
    public class CustomerGetViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyName { get; set; }
        public DateTime LastModified { get; set; }
        public ICollection<AddressGetViewModel> Addresses { get; set; } = new List<AddressGetViewModel>();
    }
}
