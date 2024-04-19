using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using KooliProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using KooliProjekt.IntegrationTests.Helpers;
using System.Net;
using System;

namespace KooliProjekt.IntegrationTests
{
    internal class PostTests : TestBase
    {
        private readonly CustomerController _controller;
        private readonly InvoiceController _invoiceController;
        private readonly OrderController _orderController;
        private readonly ProductController _productController;
        private readonly ICustomerService _customerService;
        private readonly IInvoiceService _invoiceService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ApplicationDbContext _context;

        public PostTests()
        {
            _invoiceController = new InvoiceController(_context, _orderService, _invoiceService, _productService, _customerService);
            _orderController = new OrderController(_context, _imageService, _orderService, _productService, _customerService);
            _productController = new ProductController(_imageService, _productService, _orderService);
            _controller = new CustomerController(_customerService);
        }
        [Theory]
        [InlineData("/Customer/Index")]
        [InlineData("/Customer/AdminPanel")]
        [InlineData("/Customer/Details")]
        [InlineData("/Invoice/Index")]
        [InlineData("/Order/Index")]
        [InlineData("/Order/Create")]
        [InlineData("/Product/Create")]

    }
}
