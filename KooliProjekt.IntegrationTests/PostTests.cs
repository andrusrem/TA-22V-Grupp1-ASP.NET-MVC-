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
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using NuGet.Protocol;
using Microsoft.AspNetCore.Http;

namespace KooliProjekt.IntegrationTests
{
    public class PostTests : TestBase
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
        [InlineData("/Invoice/Create", 1)]
        [InlineData("/Order/Create", 2)]
        [InlineData("/Product/Create", 3)]
        public async Task Post_EndPointReturn_CrearedAtActionResult(string url, int testNum)
        {
            //Arrange
            var customer = new Customer
            {
                Id = "sfew",
                Name = "Test",
                Phone = "Test",
                Address = "Test",
                City = "Test",
                Postcode = "Test",
                Country = "Test",
            };
            var product = new Product
            {
                Id = 1,
                Brand = "Test",
                Model = "Test",
                Manufacturer = "Test",
                CarNum = "Test",
                CarType = CarType.Sedan,
                DistancePrice = 1,
                TimePrice = 1,

            };
            var order = new Order
            {
                Id = 1,
                ProductId = product.Id,
                Product = product,
                ProductEstimatedPrice = 1,
                WhenTaken = DateTime.UtcNow,
                CustomerId = customer.Id,
                Customer = customer,


            };
            var invoice = new Invoice
            {
                Id = 1,
                ProductId = product.Id,
                Product = product,
                WhenTaken = DateTime.Now,
                GivenBack = DateTime.Now,
                DistanceDriven = 2,
                TotalPrice = 100,
                PayBy = DateTime.Now,
                PayStatus = false,
                CustomerId = customer.Id,
                Customer = customer,
                OrderId = order.Id,
                Order = order
            };
            StringContent data = null;
            AuthenticateAdmin();
            var dbContext = GetDbContext();
            if (testNum == 1)
            {
                await dbContext.AddAsync(customer);
                await dbContext.AddAsync(product);
                await dbContext.AddAsync(order);
                await dbContext.SaveChangesAsync();
                _invoiceController.Create(invoice);
                data = new StringContent(JsonConvert.SerializeObject(invoice), Encoding.UTF8, "application/json");

            }
            else if(testNum == 2)
            {
                await dbContext.AddAsync(customer);
                await dbContext.SaveChangesAsync();
                await dbContext.AddAsync(product);
                await dbContext.SaveChangesAsync();
                _orderController.Create(order);
                data = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
            }
            else if(testNum == 3)
            {
                await dbContext.AddAsync(customer);
                await dbContext.SaveChangesAsync();
                _productController.Create(product, image:null);
                data = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            }
            var client = Factory.CreateClient();
            //Act
            var result = await client.PostAsync(url, data);
            //Assert
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }
    }
}
