using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

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
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync((Customer?)null);
            
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
            string id = "r";
            _customerService.Setup(x => x.Existance(id)).Returns(false);

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
        public async Task Edit_returns_edited_Customer_and_Index_view_if_customer_was_edited()
        {
            //Arrange
            string id = "1fcea04f-7ac7-434a-bfbe-c37f445e377e";
            var customer = new Customer();
            _customerService.Setup(x => x.Existance(id)).Returns(true);
            _customerService.Setup(x => x.GetById(id)).ReturnsAsync(customer);
            customer.City = "Tallinn";
            _customerService.Setup(x => x.Save(id, customer));

            //Act
            var result = await _controller.Edit(id, customer);


            //Assert
            Assert.IsType<ViewResult>(result);
            // Assert.True(string.IsNullOrEmpty(result.ViewName) ||
            //             result.ViewName == "Index");


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
    }
}