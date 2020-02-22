using AdWorksCore3.Web.Controllers;
using AdWorksCore3.Web.Services;
using AdWorksCore3.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdWorksCore3.Web.Test.Controllers
{
    public class CustomersControllerTests
    {
        [Fact]
        public async Task ForCustomer_ReturnHttpNotFound_ForInvalidId()
        {
            // Arrange
            int invalidId = 99999;
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockRepository = new Mock<ICustomerRepository>();
            var controller = new CustomersController(mockLogger.Object, mockRepository.Object);

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
            var controller = new CustomersController(mockLogger.Object, mockRepository.Object);

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
            mockRepository.Setup(repo => repo.ListAsync())
                .ReturnsAsync(GetTestCustomerList(count));
            var controller = new CustomersController(mockLogger.Object, mockRepository.Object);

            // Act
            var result = await controller.List();

            // Assert
            var apiResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IEnumerable<CustomerGetViewModel>>(apiResult.Value);
            Assert.Equal(count, model.Count());
        }

        [Fact(Skip = "Need to fix testing error.")]
        public async Task ForCustomer_Create_ValidCustomer()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockRepository = new Mock<ICustomerRepository>();
            mockRepository.Setup(repo => repo.AddAsync(GetTestUpdate()))
                .ReturnsAsync(GetTestCustomer(1));
            var controller = new CustomersController(mockLogger.Object, mockRepository.Object);

            // Act
            var result = await controller.Create(GetTestUpdate());

            // Assert
            var apiResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        private CustomerUpdateViewModel GetTestUpdate()
        {
            return new CustomerUpdateViewModel
            {
                FirstName = "Betty",
                LastName = "Baker",
                EmailAddress = "bb@yahoo.com"
            };
        }

        private CustomerGetViewModel GetTestCustomer(int id)
        {
            return new CustomerGetViewModel
            {
                Id = id,
                FirstName = "Abe",
                LastName = "Abrams",
                LastModified = DateTime.Now
            };
        }

        private IEnumerable<CustomerGetViewModel> GetTestCustomerList(int count)
        {
            List<CustomerGetViewModel> list = new List<CustomerGetViewModel>(count);
            int startingId = 101;
            for(int i = startingId; i< startingId+count; i++)
            {
                list.Add(GetTestCustomer(i));
            }
            return list;
        }
    }
}
