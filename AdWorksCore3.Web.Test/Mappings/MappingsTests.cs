using AdWorksCore3.Core.Entities;
using AdWorksCore3.Web.Mappings;
using AdWorksCore3.Web.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdWorksCore3.Web.Test.Mappings
{
    public class MappingsTests
    {
        private readonly Mapper mapper;

        public MappingsTests()
        {
            var mapConfig = new MapperConfiguration(opt =>
            {
                opt.AddProfile<CustomersProfile>();
                opt.AddProfile<AddressProfile>();
            });

            mapper = new Mapper(mapConfig);
        }

        [Fact]
        public void MapCustomerToGetViewModelShouldBeValid()
        {
            Customer customer = new Customer
            {
                CustomerId = 9,
                ModifiedDate = DateTime.UtcNow
            };

            var vm = mapper.Map<CustomerGetViewModel>(customer);

            Assert.IsType<CustomerGetViewModel>(vm);
            Assert.Equal(customer.CustomerId, vm.Id);
            Assert.Equal(customer.ModifiedDate.Ticks, vm.LastModified.Ticks);
        }
        [Fact]
        public void MapAddressToGetViewModelShouldBeValid()
        {
            CustomerAddress address = new CustomerAddress()
            {
                AddressId = 17,
                AddressType = "Home",
                Address = new Address
                {
                    AddressId = 17,
                    AddressLine1 = "PO Box 73",
                    City = "Birmingham",
                    ModifiedDate = DateTime.UtcNow
                }
            };

            var vm = mapper.Map<AddressGetViewModel>(address);

            Assert.IsType<AddressGetViewModel>(vm);
            Assert.Equal(address.AddressId, vm.Id);
            Assert.Equal(address.AddressType, vm.AddressType);
            Assert.Equal(address.Address.AddressLine1, vm.AddressLine1);
            Assert.Equal(address.Address.City, vm.City);
            Assert.Equal(address.Address.ModifiedDate, vm.LastModified);
        }

        [Fact]
        public void MapCustomerWith1AddressToGetViewModel_ShouldBeValid()
        {
            Customer customer = new Customer
            {
                CustomerId = 19,
                ModifiedDate = DateTime.UtcNow
            };

            Address address = new Address
            {
                AddressId = 29,
                City = "Auburn",
                ModifiedDate = DateTime.UtcNow.AddMinutes(10.0)
            };

            customer.CustomerAddress.Add(new CustomerAddress
            {
                CustomerId = customer.CustomerId,
                AddressId = address.AddressId,
                AddressType = "Work",
                ModifiedDate = DateTime.UtcNow.AddMinutes(20.0),
                Customer = customer,
                Address = address
            });

            CustomerGetViewModel vm = mapper.Map<CustomerGetViewModel>(customer);

            Assert.IsType<CustomerGetViewModel>(vm);
            Assert.NotEmpty(vm.Addresses);
        }
        [Fact]
        public void MapCustomerUpdateViewModelToCustomer_ShouldBeValid()
        {
            CustomerUpdateViewModel vm = new CustomerUpdateViewModel
            {
                FirstName = "Zelda",
                LastName = "Zucker"
            };

            Customer customer = mapper.Map<Customer>(vm);

            Assert.IsType<Customer>(customer);
            Assert.Equal(vm.FirstName, customer.FirstName);
        }
        [Fact]
        public void MapCustomerUpdateViewModelWithExistingCustomer_ShouldBeUpdated()
        {
            CustomerUpdateViewModel vm = new CustomerUpdateViewModel
            {
                FirstName = "Wan",
                LastName = "Woon"
            };

            Customer customer = new Customer
            {
                CustomerId = 23,
                ModifiedDate = DateTime.UtcNow,
                FirstName = "Hector",
                LastName = "Herrera"
            };

            mapper.Map(vm, customer);

            Assert.Equal(23, customer.CustomerId);
            Assert.Equal(vm.FirstName, customer.FirstName);
            Assert.Equal(vm.LastName, customer.LastName);
        }
    }
}
