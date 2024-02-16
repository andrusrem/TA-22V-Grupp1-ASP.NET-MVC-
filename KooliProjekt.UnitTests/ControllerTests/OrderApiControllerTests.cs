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
    }
}