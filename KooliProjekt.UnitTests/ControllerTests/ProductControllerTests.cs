using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class ProductControllerTests
    {
        private readonly ProductController _controller;
        private readonly Mock<IProductService> _productService;
        private readonly Mock<IOrderService> _orderService;
        private readonly Mock<IImageService> _imageService;

        public ProductControllerTests()
        {
            _productService = new Mock<IProductService>();
            _orderService = new Mock<IOrderService>();
            _imageService = new Mock<IImageService>();
            _controller = new ProductController(_imageService.Object, 
                                                _productService.Object, 
                                                _orderService.Object);
        }

        [Fact]
        public async void Index_Return_View() {
            // Arrange
            _productService.Setup(x => x.List(1, 5)).ReturnsAsync(new PagedResult<Product>());

            // Act
            var result = await _controller.Index(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Details_Return_Not_Found_When_Id_Is_Missing() {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Details_Return_Not_Found_When_Product_Does_Not_Exist() {
            // Arrange
            int id = 1;
            _productService.Setup(x => x.Existance(id)).Returns(false);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Details_Return_Not_Found_When_Product_Does_Not_Exist2() {
            // Arrange
            int id = 1;
            _productService.Setup(x => x.Existance(id)).Returns(true);
            _productService.Setup(x => x.GetById(id)).ReturnsAsync(null as Product);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Details_Return_View_When_Product_Exists() {
            // Arrange
            int id = 1;
            _productService.Setup(x => x.Existance(id)).Returns(true);
            _productService.Setup(x => x.GetById(id)).ReturnsAsync(new Product());

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Create_Return_View() {
            // Act
            var result = _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Create_Invalid_ModelState_Return_View() {
            // Arrange
            _controller.ModelState.AddModelError(nameof(Product), "Name is required");

            // Act
            var result = await _controller.Create(new Product(), null) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Create_Save_Product_And_Return_To_Index() {
            // Arrange
            var product = new Product();
            var image = new Mock<IFormFile>();
            var readStream = new Mock<Stream>();
            _productService.Setup(x => x.Save(product)).Verifiable();
            _imageService.Setup(x => x.WriteImage(product.Id, readStream.Object)).Verifiable();
            image.Setup(x => x.OpenReadStream()).Returns(readStream.Object);

            // Act
            var result = await _controller.Create(product, image.Object) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _productService.VerifyAll();
            _imageService.VerifyAll();
        }

        [Fact]
        public void Image_Return_File() {
            // Arrange
            int id = 1;
            var stream = new Mock<Stream>();
            _imageService.Setup(x => x.ReadImage(id)).Returns(stream.Object);

            // Act
            var result = _controller.Image(id) as FileResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_Return_Not_Found_When_Id_Is_Missing() {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_Return_Not_Found_When_Product_Does_Not_Exist() {
            // Arrange
            int id = 1;
            _productService.Setup(x => x.Existance(id)).Returns(true);
            _productService.Setup(x => x.GetById(id)).ReturnsAsync(null as Product);

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_Return_View_When_Product_Exists() {
            // Arrange
            int id = 1;
            _productService.Setup(x => x.Existance(id)).Returns(true);
            _productService.Setup(x => x.GetById(id)).ReturnsAsync(new Product());

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_Save_Product_And_Return_To_Index() {
            // Arrange
            var product = new Product();
            var image = new Mock<IFormFile>();
            var readStream = new Mock<Stream>();
            _productService.Setup(x => x.Save(product)).Verifiable();
            _imageService.Setup(x => x.UpdateImage(product.Id, readStream.Object)).Verifiable();
            image.Setup(x => x.OpenReadStream()).Returns(readStream.Object);

            // Act
            var result = await _controller.Edit(product.Id, product, image.Object) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _productService.VerifyAll();
            _imageService.VerifyAll();
        }

        [Fact]
        public async void Edit_Return_NotFound_If_Id_Is_Wrong() {
            // Arrange
            int id = 1;
            var product = new Product();

            // Act
            var result = await _controller.Edit(id, product, null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_ModelState_Invalid_Return_View() {
            // Arrange
            int id = 1;
            var product = new Product { Id = id };
            _controller.ModelState.AddModelError(nameof(Product), "Name is required");

            // Act
            var result = await _controller.Edit(id, product, null) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_Error_Save_And_Return_NotFound() {
            // Arrange
            int id = 1;
            var product = new Product { Id = id };
            _productService.Setup(x => x.Save(product)).Throws(new DbUpdateConcurrencyException());
            _productService.Setup(x => x.Existance(id)).Returns(false);

            // Act
            var result = await _controller.Edit(id, product, null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_Error_Save_And_Catch_Error() {
            // Arrange
            int id = 1;
            var product = new Product { Id = id };
            _productService.Setup(x => x.Save(product)).Throws(new DbUpdateConcurrencyException());
            _productService.Setup(x => x.Existance(id)).Returns(true);

            // Act
            var catched = false;
            try {
                var result = await _controller.Edit(id, product, null) as RedirectToActionResult;
            } catch(DbUpdateConcurrencyException) {
                catched = true;
            }
            

            // Assert
            Assert.True(catched);
        }

        [Fact]
        public async void Edit_Save_Without_Image_And_Return_To_Index() {
            // Arrange
            var product = new Product();
            _productService.Setup(x => x.Save(product)).Verifiable();

            // Act
            var result = await _controller.Edit(product.Id, product, null) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _productService.VerifyAll();
        }
        
        [Fact]
        public async void Delete_Return_Not_Found_When_Id_Is_Missing() {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Delete_Return_Not_Found_When_Product_Does_Not_Exist() {
            // Arrange
            int id = 1;
            _productService.Setup(x => x.Existance(id)).Returns(true);
            _productService.Setup(x => x.GetById(id)).ReturnsAsync(null as Product);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Delete_Return_View_When_Product_Exists() {
            // Arrange
            int id = 1;
            _productService.Setup(x => x.Existance(id)).Returns(true);
            _productService.Setup(x => x.GetById(id)).ReturnsAsync(new Product());

            // Act
            var result = await _controller.Delete(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void DeleteConfirmed_Delete_Product_And_Return_To_Index() {
            // Arrange
            int id = 1;
            _productService.Setup(x => x.Delete(id)).Verifiable();

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _productService.VerifyAll();
        }
    }
}
