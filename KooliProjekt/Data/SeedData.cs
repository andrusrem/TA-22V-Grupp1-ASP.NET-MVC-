using KooliProjekt.Data;

namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext applicationDbContext)
        {
            applicationDbContext.Database.EnsureCreated();
            if(applicationDbContext.Invoices.Count() > 0){
                return;
            }

            GenerateProduct(applicationDbContext);
            GenerateOrder(applicationDbContext);
            GenerateInvoice(applicationDbContext);

            applicationDbContext.SaveChanges();
        }

        private static void GenerateProduct(ApplicationDbContext applicationDbContext)
        {
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
            var order = new Order();
            order.ProductId = 3;
            order.EstimatedPrice = 1495;

            applicationDbContext.Orders.Add(order);
        }
        private static void GenerateInvoice(ApplicationDbContext applicationDbContext)
        {
            var invoice = new Invoice();
            invoice.ProductId = 4;
            invoice.TotalPrice = 100;
            invoice.DistanceDriven = 10;
            invoice.PayStatus = false;
            invoice.CustomerId = 1;

            applicationDbContext.Invoices.Add(invoice);

        }
    }
}