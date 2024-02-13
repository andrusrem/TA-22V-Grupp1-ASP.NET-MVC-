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
    public class KooliProjektController
    {
        private readonly Mock<IInvoiceService> _invoiceService;
        private readonly Mock<IProductService> _productService;
        private readonly Mock<ICustomerService> _customerService;
        private readonly Mock<IOrderService> _orderService;
        private readonly InvoiceApiController _controller;

        public KooliProjektController()
        {
            _invoiceService = new Mock<IInvoiceService>();
            _productService = new Mock<IProductService>();
            _customerService = new Mock<ICustomerService>();
            _orderService = new Mock<IOrderService>();
            _controller = new InvoiceApiController(_orderService.Object, 
                                                    _invoiceService.Object, 
                                                    _productService.Object, 
                                                    _customerService.Object);
        }

        [Fact]
        public async Task GetInvoices_return_invoices()
        {
            //Arrange
            _invoiceService.Setup(x => x.GetAllInvoices()).ReturnsAsync(new List<Invoice>());
            //Act
            var result = await _controller.GetInvoices();
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task GetInvoice_return_invoice_if_invoice_exist()
        {
            //Arrange
            int id = 1;
            _invoiceService.Setup(x => x.GetById(id)).ReturnsAsync(new Invoice());
            //Act
            var result = await _controller.GetInvoice(id);
            //Assert
            Assert.IsType<Invoice>(result.Value);
        }
        [Fact]
        public async Task GetInvoice_return_notFound_if_invoice_not_exist()
        {
            //Arrange
            int id = 1;
            _invoiceService.Setup(x => x.GetById(id)).ReturnsAsync((Invoice?)null);
            //Act
            var result = await _controller.GetInvoice(id);
            //Assert
            Assert.Null(result.Value);
        }
        [Fact]
        public async Task PutInvoice_return_badrequest_if_id_not_invoice_id()
        {
            //Arrange
            int id = 1;
            var invoice = new Invoice{Id = 2};
            //Act
            var result = await _controller.PutInvoice(id, invoice) as BadRequestResult;
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task PutInvoice_return_not_found_if_invoice_not_exist()
        {
            //Arrange
            int id = 1;
            var invoice = new Invoice{Id = id};
            try{
                _invoiceService.Setup(x => x.Entry(invoice)).Throws(new DbUpdateConcurrencyException());
            }
            catch(DbUpdateConcurrencyException){
                _invoiceService.Setup(x => x.Existance(id)).Returns(false);
            }
            //Act
            var result = await _controller.PutInvoice(id, invoice) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PutInvoice_return_to_Entry_if_invoice_found()
        {
            //Arrange
            int id = 1;
            var invoice = new Invoice{Id = id};
            _invoiceService.Setup(x => x.Entry(invoice)).Throws(new DbUpdateConcurrencyException());
            _invoiceService.Setup(x => x.Existance(id)).Returns(true);
            var catched = false;
            //Act
            try{
                var result = await _controller.PutInvoice(id, invoice) as NotFoundResult;
            }
            catch{
                catched = true;
            }
            
            //Assert
            Assert.True(catched);
        }

        [Fact]
        public async Task PutInvoice_return_noContent_if_invoice_found()
        {
            //Arrange
            int id = 1;
            var invoice = new Invoice{Id = id};
            _invoiceService.Setup(x => x.Entry(invoice)).Verifiable();
            //Act
            var result = await _controller.PutInvoice(id , invoice) as NoContentResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task PostInvoice_return_CreatedAction()
        {
            //Arrange
            var invoice = new Invoice();
            _invoiceService.Setup(x => x.Add(invoice)).Verifiable();
            //Act
            var result = await _controller.PostInvoice(invoice);
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteInvoice_return_Not_Found_if_invoice_not_found()
        {
            //Arrange
            int id = 1;
            _invoiceService.Setup(x => x.GetById(id)).ReturnsAsync((Invoice?)null);
            //Act
            var result = await _controller.DeleteInvoice(id) as NotFoundResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteInvoice_Detele_invoice_if_invoice_found()
        {
            //Arrange
            int id = 1;
            var invoice = new Invoice{Id = id};
            _invoiceService.Setup(x => x.GetById(id)).ReturnsAsync(invoice);
            _invoiceService.Setup(x => x.Delete(id)).Verifiable();
            //Act
            var result = await _controller.DeleteInvoice(id) as NoContentResult;
            //Assert
            Assert.NotNull(result);
        }
    }
}