using System.Diagnostics.CodeAnalysis;
namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public class Myorders
    {
        public int Id {get; set;}
        public int ProductId {get;set;}
        public string ProductName {get; set;}
        public DateTime WhenTaken {get; set;}
        public Product Product {get;set;}
        
        public decimal ProductEstimatedPrice {get; set;} //Calculated from DistancePrice and TimePrice in Product
        public string CustomerId {get;set;}
        public Customer Customer {get;set;}

        public List<Product> Products { get; set; }
    }
}