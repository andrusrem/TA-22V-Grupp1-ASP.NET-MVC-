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
    public class CustomerControllerTests
    {
        private readonly CustomerController _controller;
        private readonly Mock<ICustomerService> _customerService;

        public CustomerControllerTests()
        {
            _customerService = new Mock<ICustomerService>();
            _controller = new CustomerController(_customerService.Object);
        }
        
        [Fact]
        public async void Details_Returns_Not_Found_When_Id_Is_Missing()
        {
            // Arrange
            string? id = null;
            
            // Act
            var result = await _controller.Details(id) as NotFoundResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Details_Returns_Not_Found_When_Customer_Does_Not_Exist()
        {
            // Arrange
            string id = "dfegrfde";
            var customer = new Customer();
            customer = null;
            _customerService.Setup(x => x.Existance(id)).Returns(true);
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync(customer);
            
            // Act
            var result = await _controller.Details(id) as NotFoundResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_view_and_model_if_customer_was_found()
        {
            // Arrange
            string id = "1fcea04f-7ac7-434a-bfbe-c37f445e377e";
            var customer = new Customer();
            _customerService.Setup(x => x.Existance(id)).Returns(true);
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync(customer);
            
            // Act
            var result = await _controller.Details(id) as ViewResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(string.IsNullOrEmpty(result.ViewName) ||
                        result.ViewName == "Details");
        }

        [Fact]
        public async Task Edit_returns_not_found_if_id_is_missing()
        {
            // Arrange
            string? id = null;
            
            // Act
            var result = await _controller.Edit(id) as NotFoundResult;
        
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_returns_not_found_if_customer_does_not_exist()
        {
            // A
            string id = "1";
            var customer = new Customer();
            customer = null;
            _customerService.Setup(x => x.Existance(id)).Returns(true);
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync(customer);

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_returns_view_and_model_if_customer_was_found()
        {
            // Arrange
            string id = "1fcea04f-7ac7-434a-bfbe-c37f445e377e";
            var customer = new Customer();
            _customerService.Setup(x => x.Existance(id)).Returns(true);
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync(customer);
            
            // Act
            var result = await _controller.Edit(id) as ViewResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.True(string.IsNullOrEmpty(result.ViewName) ||
                        result.ViewName == "Edit");
        }

        [Fact]
        public async Task Edit_returns_edited_Customer_and_Index_view()
        {
            //Arrange

            string id = "1fcea04f-7ac7-434a-bfbe-c37f445e377e";
            var customer = new Customer{Id = id};
            _customerService.Setup(x => x.Save(id, customer)).Verifiable();



            //Act
            var result = await _controller.Edit(id, customer) as RedirectToActionResult;
            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _customerService.VerifyAll();
        }

        [Fact]
        public async Task Edit_retruns_not_found_if_customerId_not_equal_id()
        {
            //Arrange
            string id = "1";
            var customer = new Customer();
            customer.Id = "2";
            

            //Act
            var result = await _controller.Edit(id, customer) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_Invalid_model_return_view()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{Id = id};
            _controller.ModelState.AddModelError(nameof(customer), "Error");
            //Act
            var result = await _controller.Edit(id, customer) as ViewResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_Save_and_Catch_Exeption()
        {
            //Arrange
            string id = "a";
            var customer = new Customer{Id = id};
            _customerService.Setup(x => x.Save(id, customer)).Throws(new DbUpdateConcurrencyException());
            _customerService.Setup(x => x.Existance(id)).Returns(true);
            //Act
            var catched = false;
            try
            {
                var result = await _controller.Edit(id, customer) as RedirectToActionResult;
            }
            catch (DbUpdateConcurrencyException )
            {
                catched = true;
            }
            
            //Assert
            Assert.True(catched);
        }
        [Fact]
        public async Task Edit_return_NotFound_when_Save_and_Catch_Exeption()
        {
            //Arrange
            var customer = new Customer{Id = "1"};
            _customerService.Setup(x => x.Save(customer.Id, customer)).Throws(new DbUpdateConcurrencyException());
            _customerService.Setup(x => x.Existance(customer.Id)).Returns(false);
            
            //Act

            var result = await _controller.Edit(customer.Id, customer) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void AdminPanel_return_view_if_customer_is_Admin()
        {
            //Arrange
            string id = "1";
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), new string[] { "Admin" })
            };
            
            //Act
            var result = _controller.AdminPanel() as ViewResult;
            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task Index_returns_view_and_model()
        {
            // Arrange
            var customers = new List<Customer>();
            _customerService.Setup(x => x.GetCustomerAsync()).ReturnsAsync(customers);
            
            // Act
            var result = await _controller.Index() as ViewResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
        }

        [Fact]
        public async Task Delete_return_NotFound_if_Id_is_missing()
        {
            //Arrange
            string id = null;
            _customerService.Setup(x => x.Existance(id)).Returns(false);
            //Act
            var result = await _controller.Delete(id) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Delete_return_NotFound_if_customer_not_exist()
        {
            //Arrange
            string id = "1";
            _customerService.Setup(x => x.Existance(id)).Returns(true);
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync((Customer?)null);

            //Act
           var result = await _controller.Delete(id) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Delete_returns_View_if_customer_Deleted()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{Id = id};
            _customerService.Setup(x => x.Existance(id)).Returns(true);
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync(customer);
            //Act
            var result = await _controller.Delete(id) as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_return_Index_View_after_delete()
        {
            //Arrange
            string id = "1";
            var customer = new Customer{Id = id};
            _customerService.Setup(x => x.Delete(id)).Verifiable();

            //Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

    }
}
