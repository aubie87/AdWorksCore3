using AdWorksCore3.Core.Entities;
using AdWorksCore3.Core.Interfaces;
using AdWorksCore3.Core.Services;
using AdWorksCore3.Web.Controllers.Api;
using AdWorksCore3.Web.Mappings;
using AdWorksCore3.Web.ResourceParameters;
using AdWorksCore3.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using Xunit;

namespace AdWorksCore3.Web.Test.Controllers.Api
{
    public class CustomersControllerTests
    {
        private readonly Mapper mapper;

        public CustomersControllerTests()
        {
            var mapConfig = new MapperConfiguration(opt =>
            {
                opt.AddProfile<CustomersProfile>();
                opt.AddProfile<AddressProfile>();
            });

            mapper = new Mapper(mapConfig);
        }
        [Fact]
        public async Task ForCustomer_ReturnHttpNotFound_ForInvalidId()
        {
            // Arrange
            int invalidId = 99999;
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockRepository = new Mock<ICustomerRepository>();
            var controller = new CustomersController(mockLogger.Object, mapper, mockRepository.Object);

            // Act
            var result = await controller.GetById(invalidId);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(invalidId, notFoundObjectResult.Value);
        }

        [Fact]
        public async Task ForCustomer_ReturnCustomer_ForValidId()
        {
            // Arrange
            int validId = 9;
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(repo => repo.GetByIdAsync(validId))
                .ReturnsAsync(GetTestCustomer(validId));
            var controller = new CustomersController(mockLogger.Object, mapper, mockRepository.Object);

            // Act
            var result = await controller.GetById(validId);

            // Assert
            var apiResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<CustomerGetViewModel>(apiResult.Value);
            Assert.Equal(validId, model.Id);
        }

        [Fact]
        public async Task ForCustomer_ReturnList()
        {
            int count = 5;
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(repo => repo.ListAsync(new CustomersParameters()))
                .ReturnsAsync(await GetTestCustomerList(count));
            var controller = new CustomersController(mockLogger.Object, mapper, mockRepository.Object);

            // Act
            var result = await controller.GetPage(new CustomersParameters());

            // Assert
            var apiResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<CustomerGetViewModel>>(apiResult.Value);
            Assert.Equal(count, model.Count());
        }

        [Fact]
        public async Task ForCustomer_Create_ValidCustomer()
        {
            // Arrange
            int newId = 101;
            Customer retCustomer = GetTestCustomer(newId);
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockRepository = new Mock<ICustomerRepository>();
            // Tell Moq to match on any customer instance
            mockRepository.Setup(repo => repo.AddAsync(It.IsAny<Customer>()))
                .ReturnsAsync(retCustomer);
            var controller = new CustomersController(mockLogger.Object, mapper, mockRepository.Object);

            // Act
            var result = await controller.Create(CustomerUpdateViewModel.FromCustomerEntity(GetTestUpdate()));

            // Assert
            var apiResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var model = Assert.IsAssignableFrom<CustomerGetViewModel>(apiResult.Value);
            Assert.Equal(newId, model.Id);
        }

        [Fact]
        public async Task CustomerList_ShouldReturn_PagedList()
        {
            int count = 5;
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(repo => repo.ListAsync(new CustomersParameters()))
                .ReturnsAsync(await GetTestCustomerList(count));
            var controller = new CustomersController(mockLogger.Object, mapper, mockRepository.Object);

            var customerParams = new CustomersParameters { PageNumber = 2, PageSize = 2 };

            // Act
            var result = await controller.GetPage(customerParams);

            // Assert
            var apiResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<CustomerGetViewModel>>(apiResult.Value);
            Assert.Equal(customerParams.PageSize, model.Count());
        }

        private Customer GetTestUpdate()
        {
            Debug.WriteLine("GetTestUpdate - no ID");
            return new Customer
            {
                FirstName = "Betty",
                LastName = "Baker",
                EmailAddress = "bb@yahoo.com"
            };
        }

        private Customer GetTestCustomer(int id)
        {
            Debug.WriteLine("GetTestCustomer - with id=" + id);
            Customer customer = GetTestUpdate();
            customer.CustomerId = id;
            customer.Rowguid = Guid.Parse("48919bc6-1c78-41e7-b2c9-3804dc49c552");
            return customer;
        }

        private async Task<IPagedList<Customer>> GetTestCustomerList(int count)
        {
            IList<Customer> list = new List<Customer>(count);
            int startingId = 101;
            for(int i = startingId; i< startingId+count; i++)
            {
                list.Add(GetTestCustomer(i));
            }
            return await list.ToPagedListAsync<Customer>(1, 50);
        }
    }
}
