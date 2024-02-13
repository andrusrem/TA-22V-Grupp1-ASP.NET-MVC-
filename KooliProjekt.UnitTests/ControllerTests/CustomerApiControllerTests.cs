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
        public async Task GetCustomer_return_not_found_if_customer_not_exist()
        {
            //Arrange
            string id = "1";
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync((Customer?)null);
            //Act
            var result = await _controller.GetCustomer(id);
            //Assert
            Assert.Null(result.Value);
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

        [Fact]
        public async Task PutCustomer_return_badrequest_if_id_is_not_customer_id()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{Id="2"};
            
            //Act
            var result = await _controller.PutCustomer(id, customer) as BadRequestResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PutCustomer_return_not_found_if_customer_not_exist()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{Id = id};
            try{
                _customerService.Setup(x => x.Entry(id, customer)).Throws(new DbUpdateConcurrencyException());
            }
            catch(DbUpdateConcurrencyException)
            {
                _customerService.Setup(x => x.Existance(id)).Returns(false);
            }
            
            //Act
            var result = await _controller.PutCustomer(id,customer) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task PutCustomer_return_to_Entry_if_exeption_was_throwed()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{Id = id};
            _customerService.Setup(x => x.Entry(id, customer)).Throws(new DbUpdateConcurrencyException());
            _customerService.Setup(x => x.Existance(id)).Returns(true);
            var catched = false;
            try
            {
                var result = await _controller.PutCustomer(id,customer) as NoContentResult;
            }
            catch(DbUpdateConcurrencyException)
            {
                catched = true;
            }
            
            //Act
            
            //Assert
            Assert.True(catched);
        }
        [Fact]
        public async Task PutCustomer_return_NoContect_if_customer_exist()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{Id = id};
            _customerService.Setup(x => x.Entry(id, customer)).Verifiable();
            //Act
            var result = await _controller.PutCustomer(id,customer) as NoContentResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PostCustomer_Add_customer_and_create_action()
        {
            //Arrange
            string id = "1";
            var customer = new Customer();
            _customerService.Setup(x => x.Add(customer)).Verifiable();
            //Act
            var result = await _controller.PostCustomer(customer);
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PostCustomer_return_conflict_if_customer_exist()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{ Id = id };
            _customerService.Setup(x => x.Add(customer)).Throws(new DbUpdateException());
            _customerService.Setup(x => x.Existance(customer.Id)).Returns(true);
            //Act
            var result = await _controller.PostCustomer(customer);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task PostCustomer_return_new_customer_if_catch_exeption()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{ Id = id };
            _customerService.Setup(x => x.Add(customer)).Throws(new DbUpdateException());
            _customerService.Setup(x => x.Existance(customer.Id)).Returns(false);
            var catched = false;
            //Act
            try{
                var result = await _controller.PostCustomer(customer);
            }
            
            //Assert
            catch(DbUpdateException) {
                catched = true;
            }
            Assert.True(catched);
        }

        [Fact]
        public async Task DeleteCustomer_return_not_found_if_customer_not_exist()
        {
            //Arrange
            string id = "1";
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync((Customer)null);
            //Act
            var result = await _controller.DeleteCustomer(id) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task DeleteCustomer_delete_customer_when_customer_exist()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{Id = id};
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync(customer);
            _customerService.Setup(x => x.Delete(id)).Verifiable();
            //Act
            var result = await _controller.DeleteCustomer(id) as NoContentResult;
            //Assert
            Assert.NotNull(result);
        }
    }
}