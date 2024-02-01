using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using KooliProjekt;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class InvoiceControllerTests
    {
        private readonly InvoiceController _controller;
        private readonly Mock<IInvoiceService> _invoiceService;
        private readonly Mock<IOrderService> _orderService;
        private readonly Mock<IProductService> _productService;
        private readonly Mock<ICustomerService> _customerService;
        private readonly ApplicationDbContext _context;

        public InvoiceControllerTests()
        {
            _invoiceService = new Mock<IInvoiceService>();
            _orderService = new Mock<IOrderService>();
            _productService = new Mock<IProductService>();
            _customerService = new Mock<ICustomerService>();
            _context = new ApplicationDbContext();
            _controller = new InvoiceController(_context, 
                                                _orderService.Object, 
                                                _invoiceService.Object, 
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
        public async void Details_Returns_Not_Found_When_Invoice_Does_Not_Exist()
        {
            // Arrange
            int id = 1;
            _invoiceService.Setup(x => x.Existance(id)).Returns(false);
            
            // Act
            var result = await _controller.Details(id) as NotFoundResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_returns_view_and_model_if_invoice_was_found()
        {
            // Arrange
            int id = 1;
            var invoice = new Invoice();
            _invoiceService.Setup(x => x.Existance(id)).Returns(true);
            _invoiceService.Setup(x => x.GetById(id)).ReturnsAsync(invoice);
            
            // Act
            var result = await _controller.Details(id) as ViewResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(invoice, result.Model);
        }
    }
}