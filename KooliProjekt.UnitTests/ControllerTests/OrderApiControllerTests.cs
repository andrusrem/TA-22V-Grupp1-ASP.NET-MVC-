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
using Microsoft.AspNetCore.Http.HttpResults;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class OrderApiControllerTests
    {
        private readonly Mock<IOrderService> _orderService;
        private readonly OrderApiController _controller;
        public OrderApiControllerTests()
        {
            _orderService = new Mock<IOrderService>();
            _controller = new OrderApiController(_orderService.Object);
        }

        [Fact]
        public async Task GetOrders_return_all_Orders()
        {
            //Arrange
            _orderService.Setup(x => x.GetAllOrders()).ReturnsAsync(new List<Order>());
            //Act
            var result = await _controller.GetOrders();
            //Assert
            Assert.NotNull(result);
        }
        [Fact]

        public async Task GetOrder_return_Order_if_Order_exist(){
            //Arrange
            int id = 123;
            _orderService.Setup(x => x.GetById(id)).ReturnsAsync(new Order());
            //Act
            var result = await _controller.GetOrder(id);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result.Value);
        }
        [Fact]
        public async Task GetOrder_return_NotFound_if_Order_isNull(){
            //Arrange
            int id = 123;
            _orderService.Setup(x => x.GetById(id)).ReturnsAsync((Order?)null);
            //Act
            var result = await _controller.GetOrder(id);
            //Assert
            Assert.Null(result.Value);
        }

        [Fact]

        public async Task PutOrder_return_BadRequest_if_id_NotEqual_orderId() {
            //Arrange
            int id = 123;
            var order = new Order();
            //Act
            var result = await _controller.PutOrder(id, order);
            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(id, order.Id);
        }
        [Fact]
        public async Task PutOrder_return_Entry_if_order_Exist(){
            //Arrange
            int id = 0;
            var order = new Order();
            _orderService.Setup(x => x.Entry(order)).Throws(new DbUpdateConcurrencyException());
            _orderService.Setup(x => x.Existance(id)).Returns(true);
            var exists = false;
            //Act
            try{
                var result = await _controller.PutOrder(id, order);
            }
            catch{
                exists = true;
            }
            //Assert
            Assert.True(exists);
        }
        [Fact]
        public async Task PutOrder_return_NotFound_if_order_Not_exists(){
            //Arrange
            int id = 0;
            var order = new Order();
            try{
                _orderService.Setup(x => x.Entry(order)).Throws(new DbUpdateConcurrencyException());
            }
            catch{
                _orderService.Setup(x => x.Existance(id)).Returns(false);
            }
            //Act
            var result = await _controller.PutOrder(id, order) as NotFoundResult;
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutOrder_return_NoContent_if_Entry_NotThrow_Error(){
            //Arrange
            int id = 0;
            var order = new Order();
            _orderService.Setup(x => x.Entry(order)).Verifiable();
            //Act
            var result = await _controller.PutOrder(id, order) as NoContentResult;
            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]

        public async Task PostOrder_return_CreatedAtAction_if_Added_order(){
            //Arrange
            var order = new Order();
            _orderService.Setup(x => x.Add(order)).Verifiable();
            //Act
            var result = await _controller.PostOrder(order);
            //Assert
            Assert.NotNull(result);
        }

        [Fact]

        public async Task DeleteOrder_return_NotFound_if_Order_isNull(){
            //Arrange
            int id = 213;
            _orderService.Setup(x => x.GetById(id)).ReturnsAsync((Order?)null);
            //Act
            var result = await _controller.DeleteOrder(id) as NotFoundResult;
            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]

        public async Task DeleteOrder_return_NoContent_if_order_NotNull(){
            //Arrange
            int id = 213;
            _orderService.Setup(x => x.GetById(id)).ReturnsAsync(new Order());
            _orderService.Setup(x => x.Delete(id)).Verifiable();
            //Act
            var result = await _controller.DeleteOrder(id) as NoContentResult;
            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}