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
using Azure;
using Microsoft.CodeAnalysis;
using System.Data.Common;

namespace KooliProjekt.UnitTests.ServiceTests{
    public class OrderServiceTests{
        private readonly Mock<IOrderRepository> _mockRepository;
        private readonly OrderService _service;
        private readonly Mock<IUnitOfWork> _mockunitOfWork;


        public OrderServiceTests()
        {
            _mockRepository = new Mock<IOrderRepository>();
            _mockunitOfWork = new Mock<IUnitOfWork>();
            _mockunitOfWork.SetupGet(uow => uow.OrderRepository).Returns(_mockRepository.Object);
            _service = new OrderService(_mockunitOfWork.Object);
        }
        
        [Fact]
        public async Task List_return_result_of_page(){
            //Arrange  
            PagedResult<Order> result = new PagedResult<Order>();
            int page = 1;
            int pageSize = 15;
            _mockRepository.Setup(u => u.List(page, pageSize)).ReturnsAsync(result);
            //Act
            var res = await _service.List(page, pageSize);
            //Assert
            Assert.Equal(result, res);
        }

        [Fact]
        public async Task GetAllOrders_Returns_List_Of_Orders(){
            //Arrange
            List<Order> list = new List<Order>{new Order {Id = 12345, }, new Order {Id = 56432}};
            _mockRepository.Setup(u => u.GetAllOrders()).ReturnsAsync(list);
            //Act
            var result = await _service.GetAllOrders();
            //Assert
            Assert.Equal(list, result);
        }

        [Fact]

        public async Task GetCustomerOrder_Returns_Customer_Order(){
            //Arrange
            string email = "1234@gmail.com";
            Customer customer = new Customer { Id = "1", Email = email };
            Order order = new Order { Id = 1, CustomerId = customer.Id };
            _mockRepository.Setup(u => u.GetCustomerOrders(email)).ReturnsAsync(new List<Order> { order });
            //Act
            var result = await _service.GetCustomerOrders(email);
            //Assert
            Assert.Equal(order, result[0]);
        }

        [Fact]

        public async Task GetById_Returns_Order(){
            //Arrange
            int id = 1;
            Order order = new Order{Id = id};
            _mockRepository.Setup(u => u.GetById(id)).ReturnsAsync(order);
            //Act
            var result = await _service.GetById(id);
            //Assert
            Assert.Equal(order, result);
        }

        [Fact]

        public async Task Save_saves_order(){
            //Arrange
            Order order = new Order{Id = 1};
            _mockRepository.Setup(u => u.Save(order)).Verifiable();
            //Act
            await _service.Save(order);
            //Assert
            _mockRepository.VerifyAll();
        }

        [Fact]

        public async Task Delete_deletes_order(){
            //Arrange
            int id = 1;
            _mockRepository.Setup(u => u.Delete(id)).Verifiable();
            //Act
            await _service.Delete(id);
            //Assert
            _mockRepository.VerifyAll();
        }

        [Fact]

        public void Existance_return_true()
        {
            //Arrange
            int id = 1;
            Order order = new Order { Id = 1 };
            _mockunitOfWork.Setup(u => u.OrderRepository.Existance(id)).Returns(true);
            //Act
            var result =_service.Existance(id);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void Existance_return_false()
        {
            //Arrange
            int id = 1;
            _mockRepository.Setup(u => u.Existance(id)).Returns(false);
            //Act
            var result = _service.Existance(id);
            //Assert
            Assert.False(result);
        }

        [Fact]

        public async Task Entry_makes_Entry(){
            //Arrange
            Order order = new Order();
            _mockRepository.Setup(u => u.Entry(order)).Verifiable();
            //Act
            await _service.Entry(order);
            //Assert
            _mockRepository.VerifyAll();
        }

        [Fact]

        public async Task Add_Add_Order(){
            //Arrange
            int id = 1;
            Order order = new Order{Id = id};
            _mockRepository.Setup(u => u.Add(order)).Verifiable();
            //Act
            await _service.Add(order);
            //Assert
            _mockRepository.VerifyAll();
        }
    }
}