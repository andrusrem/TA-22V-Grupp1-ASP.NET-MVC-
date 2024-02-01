using KooliProjekt.Data;
using System.Diagnostics.CodeAnalysis;


namespace KooliProjekt
{
    [ExcludeFromCodeCoverage]
    public class Order : Entity
    {

        public int Id {get; set;}
        public int ProductId {get;set;}
        public decimal ProductEstimatedPrice { get; set; }
        public DateTime? WhenTaken {get; set;}
        public Product Product {get;set;}
        public string CustomerId {get;set;}
        public Customer Customer {get;set;}
        public List<Product> Products { get; set; } // This class might be needed if I start extend Order class
    }
}