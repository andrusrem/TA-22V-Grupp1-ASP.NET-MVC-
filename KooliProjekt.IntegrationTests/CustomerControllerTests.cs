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
using System.Net;

namespace KooliProjekt.IntegrationTests
{
    public class CustomerControllerTests : TestBase
    {
        private readonly CustomerController _controller;
        private readonly ICustomerService _customerService;

        public CustomerControllerTests()
        {
            _controller = new CustomerController(_customerService);
        }

        [Theory]
        [InlineData("/Customer")]
        public async Task Get_EndpointsReturn_notFound(string url)
        {
            // Arrange
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
    
}