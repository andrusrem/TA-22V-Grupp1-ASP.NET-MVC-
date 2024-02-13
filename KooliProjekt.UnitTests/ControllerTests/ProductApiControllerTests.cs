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
    public class ProductApiControllerTests
    {
        private readonly Mock<IProductService> _productService;
        private readonly ProductApiController _controller;
        public ProductApiControllerTests()
        {
            _productService = new Mock<IProductService>();
            _controller = new ProductApiController(_productService.Object);
        }

        [Fact]
        public async Task GetProducts_return_all_products()
        {
            //Arrange
            _productService.Setup(x => x.GetAllProducts()).ReturnsAsync(new List<Product>());
            //Act
            var result = await _controller.GetProducts();
            //Assert
            Assert.IsType<List<Product>>(result.Value);
        }

        [Fact]
        public async Task GetProduct_return_product()
        {
            //Arrange
            int id = 1;
            var product = new Product{Id = id};
            _productService.Setup(x => x.GetById(id)).ReturnsAsync(product);
            //Act
            var result = await _controller.GetProduct(id);
            //Assert
            Assert.Equal(product, result.Value);
        }

        [Fact]
        public async Task GetProduct_return_notFound_if_product_not_exist()
        {
            //Arrange
            int id = 1;
            _productService.Setup(x => x.GetById(id)).ReturnsAsync((Product?)null);
            //Act
            var result = await _controller.GetProduct(id);
            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task PutProduct_return_badRequest_if_id_not_product_id()
        {
            //Arrange
            int id = 1;
            var product = new Product{Id = 2};
            //Act
            var result = await _controller.PutProduct(id, product) as BadRequestResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PutProduct_return_notFound_if_product_not_exist()
        {
            //Arrange
            int id = 1;
            var product = new Product{Id = id};
            try{
                _productService.Setup(x => x.Entry(product)).Throws(new DbUpdateConcurrencyException());
            }
            catch{
                _productService.Setup(x => x.Existance(id)).Returns(false);
            }

            //Act
            var result = await _controller.PutProduct(id, product) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task PutProduct_return_Entry_if_product_exist()
        {
            //Arrange
            int id = 1;
            var product = new Product{Id = id};
            _productService.Setup(x => x.Entry(product)).Throws(new DbUpdateConcurrencyException());
            _productService.Setup(x => x.Existance(id)).Returns(true);
            var catched = false;
            //Act
            try{
                var result = await _controller.PutProduct(id, product);
            }
            catch{
                catched = true;
            }
            //Assert
            Assert.True(catched);
        }

        [Fact]
        public async Task PutProduct_return_noContent_if_Entry_not_throw_exeption()
        {
            //Arrange
            int id = 1;
            var product = new Product{Id = id};
            _productService.Setup(x => x.Entry(product)).Verifiable();

            //Act
            var result = await _controller.PutProduct(id, product) as NoContentResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PostProduct_return_CreatedAction()
        {
            //Arrange
            var product = new Product();
            _productService.Setup(x => x.Add(product)).Verifiable();
            //Act
            var result = await _controller.PostProduct(product);
            //Assert
            Assert.NotNull(result);
        } 

        [Fact]
        public async Task DeleteProduct_return_notFound_if_Product_not_found()
        {
            //Arrange
            int id = 1;
            _productService.Setup(x => x.GetById(id)).ReturnsAsync((Product?)null);
            //Act
            var result = await _controller.DeleteProduct(id) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteProduct_return_NoContent_if_product_deleted()
        {
            //Arrange
            int id = 1;
            _productService.Setup(x => x.GetById(id)).ReturnsAsync(new Product{Id = id});
            _productService.Setup(x => x.Delete(id)).Verifiable();
            //Act
            var result = await _controller.DeleteProduct(id) as NoContentResult;
            //Assert
            Assert.NotNull(result);
        }
    }
}