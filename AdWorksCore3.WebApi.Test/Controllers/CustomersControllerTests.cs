using AdWorksCore3.WebApi.Controllers;
using AdWorksCore3.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdWorksCore3.WebApi.Test.Controllers
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
    }
}
