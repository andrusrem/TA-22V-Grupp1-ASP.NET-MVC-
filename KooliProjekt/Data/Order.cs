using KooliProjekt.Data;

namespace KooliProjekt
{
    public class Order
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