using KooliProjekt.Data;
using Microsoft.AspNetCore.Identity;
using KooliProjekt.Controllers;
using KooliProjekt.Services;
using System.Security.Claims;
using System.Web;

namespace KooliProjekt.Data
{
    
    public static class SeedData
    {

        public static void Generate(ApplicationDbContext applicationDbContext,UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager)
        {
            applicationDbContext.Database.EnsureCreated();
            

            GenerateProduct(applicationDbContext);
            GenerateOrder(applicationDbContext);
            GenerateInvoice(applicationDbContext);
            
            GenerateRole(roleManager);
            AddRole(userManager);

            applicationDbContext.SaveChanges();

            
        }

        private static void GenerateProduct(ApplicationDbContext applicationDbContext)
        {
            if(applicationDbContext.Invoices.Count() > 0){
                return;
            }
            var product = new Product();
            product.Brand = "Audi";
            product.Model = "A4";
            product.CarNum = "345DCP";
            product.Manufacturer = "Germany";
            product.DistancePrice = 1;
            product.TimePrice = 2;
            applicationDbContext.Products.Add(product);
        }

        private static void GenerateOrder(ApplicationDbContext applicationDbContext)
        {
            if(applicationDbContext.Invoices.Count() > 0){
                return;
            }
            var order = new Order();
            order.ProductId = 3;
            order.ProductEstimatedPrice = 1495;

            applicationDbContext.Orders.Add(order);
        }
        private static void GenerateInvoice(ApplicationDbContext applicationDbContext)
        {
            if(applicationDbContext.Invoices.Count() > 0){
                return;
            }
            var invoice = new Invoice();
            invoice.ProductId = 4;
            invoice.TotalPrice = 100;
            invoice.DistanceDriven = 10;
            invoice.PayStatus = false;
            invoice.CustomerId = "1fcea04f-7ac7-434a-bfbe-c37f445e377e";

            applicationDbContext.Invoices.Add(invoice);

        }

        private static void GenerateRole(RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.Roles.Count() > 0)
            {
                return;
            }

            var newRole1 = new IdentityRole();
            newRole1.Name = "Admin";
            newRole1.Id = "1";
            roleManager.CreateAsync(newRole1).Wait();

            var newRole2 = new IdentityRole();
            newRole2.Name = "User";
            newRole2.Id = "2";

            roleManager.CreateAsync(newRole2).Wait();
        }

        private static void AddRole(UserManager<Customer> userManager)
        {
            //MANUALY ADD ROLE TO USER
            // var user1 = userManager.FindByEmailAsync("gogi@gmail.com").Result;
            // userManager.AddToRoleAsync(user1, "User").GetAwaiter().GetResult();

        
        }
        
    }
}