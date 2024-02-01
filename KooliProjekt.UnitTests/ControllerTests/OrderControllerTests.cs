using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class OrderControllerTests
    {
        private readonly OrderController _controller;
        private readonly Mock<IOrderService> _orderService;
        private readonly Mock<IProductService> _productService;
        private readonly Mock<ICustomerService> _customerService;
        private readonly ApplicationDbContext _context;
        private readonly Mock<IImageService> _imageService;

        public OrderControllerTests()
        {
            _orderService = new Mock<IOrderService>();
            _productService = new Mock<IProductService>();
            _customerService = new Mock<ICustomerService>();
            _context = new ApplicationDbContext();
            _imageService = new Mock<IImageService>();
            _controller = new OrderController(_context,
                                                _imageService.Object, 
                                                _orderService.Object, 
                                                _productService.Object,
                                                _customerService.Object);
        }
        
        [Fact]
        public async void Details_Returns_Not_Found_When_Id_Is_Missing()
        {
            // Arrange
            int? id = null;
            
            // Act
            var result = await _controller.Details(id) as NotFoundResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Details_Returns_Not_Found_When_Order_Does_Not_Exist()
        {
            // Arrange
            int id = 1;
            _orderService.Setup(x => x.Existance(id)).Returns(false);
            
            // Act
            var result = await _controller.Details(id) as NotFoundResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_view_and_model_if_order_was_found()
        {
            // Arrange
            int id = 1;
            var order = new Order();
            _orderService.Setup(x => x.Existance(id)).Returns(true);
            _orderService.Setup(x => x.GetById(id)).ReturnsAsync(order);
            
            // Act
            var result = await _controller.Details(id) as ViewResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(order, result.Model);
        }

        // [Fact]
        // public async Task Create_returns_view_and_model()
        // {
        //     // Arrange
        //     var order = new Order();
        //     _orderService.Setup(x => x.Save()).Returns(order);
            
        //     // Act
        //     var result = await _controller.Create() as ViewResult;
            
        //     // Assert
        //     Assert.NotNull(result);
        //     Assert.Equal(order, result.Model);
        // }

        // [Fact]
        // public async Task Create_returns_bad_request_if_model_is_invalid()
        // {
        //     // Arrange
        //     var order = new Order();
        //     _controller.ModelState.AddModelError("error", "error");
            
        //     // Act
        //     var result = await _controller.Create(order) as BadRequestResult;
            
        //     // Assert
        //     Assert.NotNull(result);
        // }
    }
}