using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

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
    }
}