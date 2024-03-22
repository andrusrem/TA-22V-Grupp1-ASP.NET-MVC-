using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using KooliProjekt.IntegrationTests.Helpers;
using System.Threading.Tasks;

namespace KooliProjekt.IntegrationTests
{
    public class CustomerControllerTests : TestBase
    {
        private readonly CustomerController _controller;
        private readonly ICustomerService _customerService;

        public CustomerControllerTests(ICustomerService customerService)
        {
            _customerService = customerService;
            _controller = new CustomerController(_customerService);
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Customer/Index")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }
    }
    
}