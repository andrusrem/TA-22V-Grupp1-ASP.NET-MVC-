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
using Microsoft.AspNetCore.Mvc.RazorPages;
 
namespace KooliProjekt.UnitTests.ServiceTests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly ProductService _service;
 
        public ProductServiceTests()
        {
            _mockRepository = new Mock<IProductRepository>();
 
            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.SetupGet(uow => uow.ProductRepository).Returns(_mockRepository.Object);
           
            _service = new ProductService( _unitOfWork.Object);
        }
        [Fact]
        public async Task List_returns_list_of_products()
        {
            //Arrange
            int page =1;
            int pageSize = 10;
            List<Product> list = new List<Product> { new Product { Id = 1343252 }, new Product { Id = 33435446 } };
            _mockRepository.Setup(u => u.List(page, pageSize)).ReturnsAsync(new PagedResult<Product> { Results = list });
            //Act
            var result = await _service.List(page, pageSize);
            //Assert
            Assert.Equal(list, result.Results);
        }
 
        [Fact]
 
        public async Task GetAllProducts_returns_products()
        {
            //Arrange
            List<Product> list = new List<Product> { new Product { Id = 1343252 }, new Product { Id = 33435446 } };
            _mockRepository.Setup(u => u.GetAllProducts()).ReturnsAsync(list);
            //Act
            var result = await _service.GetAllProducts();
            //Assert
            Assert.Equal(list, result);
 
        }
 
        [Fact]
 
        public async Task GetById_returns_product()
        {
            //Arrange
            int id = 123;
            var product = new Product { Id = id};
            _mockRepository.Setup(u => u.GetById(id)).ReturnsAsync(product);
            //Act
            var result = await _service.GetById(id);
            //Assert
            Assert.Equal(product, result);
        }
 
        [Fact]
 
        public async Task Save_save()
        {
            //Arrange
            int id = 123;
            var product = new Product { Id = id };
            _mockRepository.Setup(u => u.Save(product)).Verifiable();
            //Act
            await _service.Save(product);
            //Assert
            _mockRepository.VerifyAll();
        }
 
        [Fact]
 
        public async Task Delete()
        {
            //Arrange
            int id = 123;
            var product = new Product { Id = id};
            _mockRepository.Setup(u => u.Delete(id)).Verifiable();
            //Act
            await _service.Delete(id);
            //Assert
            _mockRepository.VerifyAll();
        }
 
        [Fact]
 
        public async Task Lookup_returns_IList()
        {
            //Arrange
            List<LookupItem> list = new List<LookupItem> { new LookupItem { Id = 1343252 }};
            _mockRepository.Setup(u => u.Lookup()).ReturnsAsync(list);
            //Act
            var result = await _service.Lookup();
            //Assert
            Assert.Equal(list, result);
           
        }
 
        [Fact]
 
        public void Existance_return_true()
        {
            //Arrange
            int id = 123;
            var product = new Product { Id = id};
            _unitOfWork.Setup(u => u.ProductRepository.Existance(id)).Returns(true);
            //Act
            var result = _service.Existance(id);
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
 
        public async Task Add_adds_product()
        {
            //Arrange
            int id = 1;
            var product = new Product { Id = id};
            _mockRepository.Setup(u => u.Add(product)).Verifiable();
            //Act
            await _service.Add(product);
            //Assert
            _mockRepository.VerifyAll();
 
        }
 
        [Fact]
 
        public async Task Entry_makes_product()
        {
            //Arrange
            int id = 1;
            var product = new Product { Id = id};
            _mockRepository.Setup(u => u.Entry(product)).Verifiable();
            //Act
            await _service.Entry(product);
            //Assert
            _mockRepository.VerifyAll();
 
        }
    }
}