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
    public class InvoiceServiceTests
    {
        private readonly Mock<IInvoiceRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly InvoiceService _service;

        public InvoiceServiceTests()
        {
            _mockRepository = new Mock<IInvoiceRepository>();

            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.SetupGet(uow => uow.InvoiceRepository).Returns(_mockRepository.Object);
            
            _service = new InvoiceService( _unitOfWork.Object);
        }

        [Fact]
        public async Task List_returns_list_of_invoices()
        {
            //Arrange
            int page =1;
            int pageSize = 10;
            List<Invoice> list = new List<Invoice> { new Invoice { Id = 1343252 }, new Invoice { Id = 33435446 } };
            _mockRepository.Setup(u => u.List(page, pageSize)).ReturnsAsync(new PagedResult<Invoice> { Results = list });
            //Act
            var result = await _service.List(page, pageSize);
            //Assert
            Assert.Equal(list, result.Results);
        }
        [Fact]
        public async Task GetCustomerInvoices_return_Customer_Invoices()
        {
            //Arrange
            string email = "a@a.com";
            Customer customer = new Customer { Id = "1", Email = "a@a.com" };
            Invoice invoice = new Invoice { Id = 1, CustomerId = customer.Id };
            _mockRepository.Setup(u => u.GetCustomerInvoices(email)).ReturnsAsync(new List<Invoice> { invoice });
            //Act
            var result = await _service.GetCustomerInvoices(email);
            //Assert
            Assert.Equal(invoice, result[0]);
        }

        [Fact]
        public async Task GetById_returns_invoice()
        {
            //Arrange
            int id = 12;
            var invoice = new Invoice { Id = id , PayStatus = true};
            _mockRepository.Setup(u => u.GetById(id)).ReturnsAsync(invoice);
            //Act
            var result = await _service.GetById(id);
            //Assert
            Assert.Equal(invoice, result);
        }
        [Fact]
        public async Task Save_save()
        {
            //Arrange
            int id = 12;
            var invoice = new Invoice { Id = 12 };
            _mockRepository.Setup(u => u.Save(invoice)).Verifiable();
            //Act
            await _service.Save(invoice);
            //Assert
            _mockRepository.VerifyAll();
        }
        [Fact]
        public async Task FindId_returns_invoice()
        {
            //Arrange
            int id = 12;
            var invoice = new Invoice { Id = id, PayStatus = true };
            _mockRepository.Setup(u => u.FindId(id)).ReturnsAsync(invoice);
            //Act
            var result = await _service.FindId(id);
            //Assert
            Assert.Equal(invoice, result);
        }
        [Fact]
        public async Task Delete()
        {
            //Arrange
            int id = 1;
            var invoice = new Invoice { Id = 1 };
            _unitOfWork.Setup(u => u.BeginTransaction()).Verifiable();
            _mockRepository.Setup(u => u.Delete(id)).Verifiable();
            _unitOfWork.Setup(u => u.Commit()).Verifiable();

            //Act
            await _service.Delete(id);

            //Assert
            _unitOfWork.VerifyAll();
            _mockRepository.VerifyAll();
        }

        [Fact]
        public void Existance_return_true()
        {
            //Arrange
            int id = 1;
            Invoice invoice = new Invoice { Id = 1 };
            _unitOfWork.Setup(u => u.InvoiceRepository.Existance(id)).Returns(true);
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
        public async Task Add_add_new_customer()
        {
            //Arrange
            int Id = 1;
            var invoice = new Invoice();
            invoice.Id = Id;
            _mockRepository.Setup(u => u.Add(invoice)).Verifiable();
            //Act
            await _service.Add(invoice);
            //Assert
            _mockRepository.VerifyAll();

        }
        [Fact]
        public async Task Entry_make_entry()
        {
            //Arrange
            int id = 1;
            var invoice = new Invoice { Id = id, PayStatus = false };
            invoice.PayStatus = true;
            _mockRepository.Setup(u => u.Entry(invoice)).Verifiable();
            //Act
            await _service.Entry(invoice);
            //Arrange
            _mockRepository.VerifyAll();
        }
        [Fact]
        public async Task GetAllInvoices_return_list_of_invoices()
        {
            //Arrange
            var invoice1 = new Invoice { Id = 1, PayStatus = false }; 
            var invoice2 = new Invoice { Id = 2, PayStatus = false };
            var list = new List<Invoice> { invoice1, invoice2 };
            _mockRepository.Setup(u => u.GetAllInvoices()).ReturnsAsync(new List<Invoice> { invoice1,invoice2 });
            //Act
            var result = await _service.GetAllInvoices();
            //Assert
            Assert.Equal(list, result);
        }
    }
}
