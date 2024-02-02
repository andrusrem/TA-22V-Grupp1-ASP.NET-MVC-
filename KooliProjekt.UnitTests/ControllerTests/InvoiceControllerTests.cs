using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using KooliProjekt;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using NuGet.Protocol;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        public async void Index_Returns_View()
        {
            // Act
            var result = await _controller.Index() as ViewResult;
            
            // Assert
            Assert.NotNull(result);
        }
        
        [Fact]
        public async void Myinvoices_Returns_View() {
            // Arrange
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), null)
            };
            _invoiceService.Setup(x => x.GetCustomerInvoices("testuser"))
                .ReturnsAsync(new List<Invoice>());

            // Act
            var result = await _controller.Myinvoices() as ViewResult;
            
            // Assert
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

        [Fact]
        public async void Details_Returns_Not_Found_When_Invoice_Is_Null() {
            // Arrange
            int id = 1;
            _invoiceService.Setup(x => x.Existance(id)).Returns(true);
            _invoiceService.Setup(x => x.GetById(id)).ReturnsAsync(null as Invoice);
            
            // Act
            var result = await _controller.Details(id) as NotFoundResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Create_If_User_Is_Admin_Return_View() {
            // Arrange
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), new string[] { "Admin" })
            };
            _productService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupItem>());
            _customerService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupCustomer>());
            
            // Act
            var result = await _controller.Create(null, null) as ViewResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Create_If_User_Is_Not_Admin_Return_Redirect_To_Myinvoices() {
            // Arrange
            var order = new Order { Id = 1, WhenTaken = DateTime.Now };
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), null)
            };
            _customerService.Setup(x => x.GetByEmail("testuser")).ReturnsAsync(new Customer());
            _productService.Setup(x => x.GetById(1)).ReturnsAsync(new Product());
            _orderService.Setup(x => x.GetById(1)).ReturnsAsync(order);
            _invoiceService.Setup(x => x.Save(It.IsAny<Invoice>())).Verifiable();
            _orderService.Setup(x => x.Delete(order.Id)).Verifiable();
            
            // Act
            var result = await _controller.Create(1, 1) as RedirectToActionResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Myinvoices", result.ActionName);
            _invoiceService.VerifyAll();
            _orderService.VerifyAll();
        }

        [Fact]
        public async void CreateConfirmed_Save_And_Redirect_To_Index() {
            // Arrange
            _invoiceService.Setup(x => x.Save(It.IsAny<Invoice>())).Verifiable();
            
            // Act
            var result = await _controller.Create(new Invoice()) as RedirectToActionResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _invoiceService.VerifyAll();
        }

        [Fact]
        public async void CreateConfirmed_Invalid_ModelState_Return_View() {
            // Arrange
            _controller.ModelState.AddModelError("error", "error");
            _productService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupItem>());
            _customerService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupCustomer>());
            
            // Act
            var result = await _controller.Create(new Invoice()) as ViewResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_Return_Not_Found() {
            // Arrange
            int? id = null;
            
            // Act
            var result = await _controller.Edit(id) as NotFoundResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_If_User_Admin_Return_View() {
            // Arrange
            int id = 1;
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), new string[] { "Admin" })
            };
            _invoiceService.Setup(x => x.Existance(id)).Returns(true);
            _invoiceService.Setup(x => x.GetById(id)).ReturnsAsync(new Invoice());
            _productService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupItem>());
            _customerService.Setup(x => x.Lookup()).ReturnsAsync(new List<LookupCustomer>());
            
            // Act
            var result = await _controller.Edit(id) as ViewResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_If_user_Admin_But_Invoice_Null_Return_NotFound() {
            // Arrange
            int id = 1;
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), new string[] { "Admin" })
            };
            _invoiceService.Setup(x => x.Existance(id)).Returns(true);
            _invoiceService.Setup(x => x.GetById(id)).ReturnsAsync(null as Invoice);
            
            // Act
            var result = await _controller.Edit(id) as NotFoundResult;
            
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Edit_Redirect_User_To_Myinvoices() {
            // Arrange
            var id = 1;
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() {
                User = new GenericPrincipal(new GenericIdentity("testuser"), new string[] { "User" })
            };
            _invoiceService.Setup(x => x.Existance(id)).Returns(true);
            _invoiceService.Setup(x => x.GetById(id)).ReturnsAsync(new Invoice());
            _invoiceService.Setup(x => x.Save(It.IsAny<Invoice>())).Verifiable();
            
            // Act
            var result = await _controller.Edit(id) as RedirectToActionResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Myinvoices", result.ActionName);
            _invoiceService.VerifyAll();
        }
    }
}
