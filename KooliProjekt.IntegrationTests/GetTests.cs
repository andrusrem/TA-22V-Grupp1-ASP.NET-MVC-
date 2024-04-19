using KooliProjekt.Controllers;
using KooliProjekt.Services;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using KooliProjekt.IntegrationTests.Helpers;
using System.Threading.Tasks;
using System.Net;
using System;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace KooliProjekt.IntegrationTests
{
    public class GetTests : TestBase
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

        public GetTests()
        {
            _invoiceController = new InvoiceController(_context, _orderService, _invoiceService, _productService, _customerService);
            _orderController = new OrderController(_context, _imageService, _orderService, _productService, _customerService);
            _productController = new ProductController(_imageService,_productService,_orderService);
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
        public async Task Get_EndpointsReturn_Forbidden_if_User_Not_Logged_In(string url)
        {
            // Arrange
            AuthenticateAnonymous();
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
        [Theory]
        [InlineData("/Customer/Index")]
        [InlineData("/Customer/AdminPanel")]
        public async Task Get_Customer_Without_Id_EndpointsReturn_Ok_if_Role_is_Admin(string url)
        {
            // Arrange
            AuthenticateAdmin();
            var client = Factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Theory]
        [InlineData("/Customer/Details/343r23trgtrew")]
        [InlineData("/Invoice/Details/1")]
        [InlineData("/Order/Details/1")]
        [InlineData("/Product/Details/1")]
        public async Task Get_Customer_With_Id_EndpointsReturn_Ok_if_Role_is_Admin(string url)
        {
            // Arrange
            string id = "343r23trgtrew";
            var customer = new Customer 
            { 
                Id = id,
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
                Product = product,
                ProductEstimatedPrice = 1,
                WhenTaken = DateTime.UtcNow,
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
            AuthenticateAdmin();
            var dbContext = GetDbContext();
            await dbContext.AddAsync(customer);
            await dbContext.SaveChangesAsync();
            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();
            await dbContext.AddAsync(order);
            await dbContext.SaveChangesAsync();
            await dbContext.AddAsync(invoice);

            await dbContext.SaveChangesAsync();

            var client = Factory.CreateClient();


            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Theory]
        [InlineData("/Invoice/Details/1")]
        [InlineData("/Order/Details/1")]
        [InlineData("/Product/Details/1")]
        public async Task Get_Invoice_With_Id_EndpointsReturn_Ok_if_Role_is_User(string url)
        {
            // Arrange
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
                Product = product,
                ProductEstimatedPrice = 1,
                WhenTaken = DateTime.UtcNow,
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
                Order  = order
            };
            AuthenticateUser();
            var dbContext = GetDbContext();
            await dbContext.AddAsync(customer);
            await dbContext.SaveChangesAsync();
            await dbContext.AddAsync(product);
            await dbContext.SaveChangesAsync();
            await dbContext.AddAsync(order);
            await dbContext.SaveChangesAsync();
            await dbContext.AddAsync(invoice);

            await dbContext.SaveChangesAsync();

            var client = Factory.CreateClient();


            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
       
    }
}
