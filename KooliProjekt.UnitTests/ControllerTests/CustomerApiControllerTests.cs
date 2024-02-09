using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class CustomerApiControllerTests
    {
        private readonly Mock<ICustomerService> _customerService;
        private readonly CustomerApiController _controller;

        public CustomerApiControllerTests()
        {
            _customerService = new Mock<ICustomerService>();
            _controller = new CustomerApiController(_customerService.Object);
        }

        [Fact]
        public async Task GetCustomer_return_customer_model()
        {
            //Arrange
            string id = "1";
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync(new Customer { Id = id });
            //Act
            var result = await _controller.GetCustomer(id);
            //Assert

            Assert.IsType<Customer>(result.Value);

        }

        [Fact]
        public async Task GetCustomers_return_customers()
        {
            //Arrange
            _customerService.Setup(x => x.GetCustomerAsync()).ReturnsAsync(new List<Customer>());
            //Act
            var result = await _controller.GetCustomers();
            //Assert
            Assert.NotNull(result);
        }
    }
}