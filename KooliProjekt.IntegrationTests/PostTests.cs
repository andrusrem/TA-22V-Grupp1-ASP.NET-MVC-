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
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http.Json;
using System.Globalization;

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
        [InlineData("/Product/Create", 1, null)]
        [InlineData("/Order/Create", 2, null)]
        [InlineData("/Invoice/Create", 3, null)]
        public async Task Create_ReturnEndPoint_CreateResult(string url, int testNum, IFormFile formFile)
        {
            //Arrange
            var customer = new Customer
            {
                Id = "s",
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
                Image = null

            };
            var order = new Order
            {
                Id = 1,
                ProductId = 1,
                ProductEstimatedPrice = 1,
                WhenTaken = DateTime.UtcNow,
                CustomerId = "s",


            };
            var invoice = new Invoice
            {
                Id = 1,
                ProductId = 1,
                WhenTaken = DateTime.Now,
                GivenBack = DateTime.Now,
                DistanceDriven = 2,
                TotalPrice = 100,
                PayBy = DateTime.Now,
                PayStatus = false,
                CustomerId = "s",
                OrderId = 1
            };
            AuthenticateAdmin();
            var dbContext = GetDbContext();
            JsonContent data = null;
            HttpResponseMessage result = null;
            if (testNum == 1)
            {
                await dbContext.AddAsync(customer);
                await dbContext.SaveChangesAsync();

                var productS = JsonContent.Create(product);
                using var memoryStream = new MemoryStream();
                memoryStream.WriteByte(65);
                memoryStream.WriteByte(66);
                memoryStream.WriteByte(67);

                using var streamContent = new StreamContent(memoryStream);

                using var fileUploadContent = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture));
                fileUploadContent.Add(productS);
                fileUploadContent.Add(streamContent);
                var client = Factory.CreateClient();
                //Act
                result = await client.PostAsync(url, fileUploadContent);

            }
            else if (testNum == 2)
            {
                await dbContext.AddAsync(customer);
                await dbContext.SaveChangesAsync();
                await dbContext.AddAsync(product);
                await dbContext.SaveChangesAsync();
                data = JsonContent.Create(order);
                var client = Factory.CreateClient();
                //Act
                result = await client.PostAsync(url, data);
            }
            else if(testNum == 3)
            {
                await dbContext.AddAsync(customer);
                await dbContext.SaveChangesAsync();
                await dbContext.AddAsync(product);
                await dbContext.SaveChangesAsync();
                await dbContext.AddAsync(order);
                await dbContext.SaveChangesAsync();
                data = JsonContent.Create(invoice);
                var client = Factory.CreateClient();
                //Act
                result = await client.PostAsync(url, data);
            }

          

            
            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

    }
}
