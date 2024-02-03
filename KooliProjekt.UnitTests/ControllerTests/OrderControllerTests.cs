using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

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
        public async Task Index_return_listed_View()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;
            _orderService.Setup(x => x.List(page, pageSize)).Verifiable();

            //Act
            var result = await _controller.Index(page) as ViewResult;

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void Image_return_image()
        {
            //Arrange
            int id = 1;
            var stream = new Mock<Stream>();
            _imageService.Setup(x => x.ReadImage(id)).Returns(stream.Object);

            //Act
            var result = _controller.Image(id) as FileResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task MyOrders_return_LoggedInUser_Orders_View()
        {
            //Arrange
             _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), null)};
            _orderService.Setup(x => x.GetCustomerOrders("testuser")).ReturnsAsync(new List<Order>());

            //Act
            var result = await _controller.Myorders() as ViewResult;

            //Assert
            Assert.NotNull(result);
            
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
            _orderService.Setup(x => x.Existance(id)).Returns(true);
            _orderService.Setup(x => x.GetById(id)).ReturnsAsync((Order?)null);
            
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

        [Fact]
        public async Task Create_create_order_if_user_is_not_Admin()
        {
            // Arrange
            var productId = 1;
            var order = new Order{Id = 1, WhenTaken = DateTime.Now};
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), new string[] {"User"})};
            _customerService.Setup(x => x.GetByEmail("testuser")).ReturnsAsync(new Customer());
            _productService.Setup(x => x.GetById(productId)).ReturnsAsync(new Product());
            
            
            // Act
            var result = await _controller.Create(productId) as RedirectToActionResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_create_order_if_user_is_Admin()
        {
            //Arrange
            var productId = 1;
            var order = new Order{Id = 1, ProductId = productId};
            order.ProductId = productId;
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), new string[] {"Admin"})};
            _productService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupItem>());
            _customerService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupCustomer>());

            //Act
            var result = await _controller.Create(productId) as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_order_and_return_Index_if_model_is_valid()
        {
            //Arrange
            _orderService.Setup(x => x.Save(It.IsAny<Order>())).Verifiable();


            //Act
            var result = await _controller.Create(new Order()) as RedirectToActionResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

        }
        [Fact]
        public async Task Create_Invalid_model_return_view()
        {
            //Arrange
            _controller.ModelState.AddModelError("error", "error");
            _productService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupItem>());
            _customerService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupCustomer>());

            //Act
            var result = await _controller.Create(new Order()) as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_returns_NotFound_if_id_missing()
        {
            //Arrange
            int? id = null;
            //Act
            var result = await _controller.Edit(id) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Edit_returns_view_id_exist()
        {
            //Arrange
            int id = 1;
            var order = new Order{Id = id};
            _orderService.Setup(x => x.Existance(order.Id)).Returns(true);
            _orderService.Setup(x => x.GetById(order.Id)).ReturnsAsync(order);
            _productService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupItem>());
            _customerService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupCustomer>());


            //Act
            var result = await _controller.Edit(id) as ViewResult;

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Edit_returns_NotFound_if_order_not_exist()
        {
            //Arrange
            int id = 1;
            _orderService.Setup(x => x.Existance(id)).Returns(true);
            _orderService.Setup(x => x.GetById(id)).ReturnsAsync((Order?)null);

            //Act
            var result = await _controller.Edit(id) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_return_NotFound_if_id_is_wrong()
        {
            //Arrange
            int id = 1;
            var order = new Order{Id = 2};

            //Act
            var result = await _controller.Edit(id, order) as NotFoundResult;

            //Assert
            Assert.NotNull(result);

        }
        [Fact]
        public async Task Edit_Save_model_and_return_Index_view()
        {
            //Arrange
            int id = 1;
            var order = new Order{Id = id};
            _orderService.Setup(x => x.Save(order)).Verifiable();
            //Act
            var result = await _controller.Edit(id, order) as RedirectToActionResult;
            //Assert
            Assert.NotNull(result);

        }
        [Fact]
        public async Task Edit_save_and_catch_exeption()
        {
            //Arrange
            int id = 1;
            var order = new Order{Id = id};
            _orderService.Setup(x => x.Save(order)).Throws(new DbUpdateConcurrencyException());
            _orderService.Setup(x => x.Existance(id)).Returns(true);
            var cathed = false;
            //Act
            try
            {
                var result = await _controller.Edit(id, order) as RedirectToActionResult;
            }
            catch (DbUpdateConcurrencyException)
            {
                cathed = true;
            }
            //Assert
            Assert.True(cathed);
        }
        [Fact]
        public async Task Edit_save_catch_exeption_and_model_not_exist()
        {
            //Arrange
            int id = 1;
            var order = new Order{Id = id};
            _orderService.Setup(x => x.Save(order)).Throws(new DbUpdateConcurrencyException());
            _orderService.Setup(x => x.Existance(id)).Returns(false);
            //Act
            var result = await _controller.Edit(id, order) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Edit_Invalid_model_return_view()
        {
            //Arrange
            int id = 1;
            var order = new Order{Id = id};
            _controller.ModelState.AddModelError("error", "error");
            _orderService.Setup(x => x.Save(order)).Verifiable();
            _productService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupItem>());
            _customerService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupCustomer>());
            //Act
            var result = await _controller.Edit(order.Id, order) as ViewResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Delete_return_NotFound_if_id_missing()
        {
            //Arrange
            int? id = null;
            //Act
            var result = await _controller.Delete(id) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Delete_return_view_if_order_deleted()
        {
            //Arrange
            int id = 1;
            var order = new Order{Id = id};
            _orderService.Setup(x => x.Existance(order.Id)).Returns(true);
            _orderService.Setup(x => x.GetById(order.Id)).ReturnsAsync(order);
            //Act
            var result = await _controller.Delete(id) as ViewResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Delete_return_NotFound_if_order_missing()
        {
            //Arrange
            int id = 1;
            _orderService.Setup(x => x.Existance(id)).Returns(true);
            _orderService.Setup(x => x.GetById(id)).ReturnsAsync((Order?)null);
            //Act
            var result = await _controller.Delete(id) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task DeleteConfirmed_return_Index_if_order_deleted()
        {
            //Arrange
            int id = 1;
            var order = new Order{Id = id};
            _orderService.Setup(x => x.Delete(order.Id)).Verifiable();
            //Act
            var result = await _controller.DeleteConfirmed(order.Id) as RedirectToActionResult;
            //Assert
            Assert.NotNull(result);
        }
    }
}