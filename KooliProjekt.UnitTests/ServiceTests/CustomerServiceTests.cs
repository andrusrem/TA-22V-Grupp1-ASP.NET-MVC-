using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using KooliProjekt.Services;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Data;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _service = new CustomerService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetCustomerAsync_returns_list_of_customers()
        {
            //Arrange
            List<Customer> list = new List<Customer> { new Customer { Id = "1343252" }, new Customer { Id = "33435446" } };
            _mockRepository.Setup(u => u.List()).ReturnsAsync(list);
            //Act
            var result = await _service.GetCustomerAsync();
            //Assert
            Assert.Equal(list, result);
        }

        [Fact]
        public async Task GetById_returns_customer()
        {
            //Arrange
            string id = "12";
            var customer = new Customer { Id = "12" };
            _mockRepository.Setup(u => u.GetById(id)).ReturnsAsync(customer);
            //Act
            var result = await _service.GetById(id);
            //Assert
            Assert.Equal(customer, result);
        }
        [Fact]
        public async Task GetByEmail_returns_customer()
        {
            //Arrange
            string email = "aa@gmail.com";
            var customer = new Customer { Email = "aa@gmail.com" };
            _mockRepository.Setup(u => u.GetByEmail(email)).ReturnsAsync(customer);
            //Act
            var result = await _service.GetByEmail(email);
            //Assert
            Assert.Equal(customer, result);
        }
        [Fact]
        public async Task Lookup_return_IList()
        {
            //Arrange
            List<LookupCustomer> list = new List<LookupCustomer> { new LookupCustomer { Name = "And", Id = "1343252" }, new LookupCustomer { Name = "Andr", Id = "33435446" } };
            _mockRepository.Setup(u => u.Lookup()).ReturnsAsync(list);
            //Act
            var result = await _service.Lookup();
            //Assert
            Assert.Equal(list, result);
        }
    }
}
