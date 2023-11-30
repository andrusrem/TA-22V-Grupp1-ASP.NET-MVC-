namespace KooliProjekt.Data
{
    public class Myorders
    {
        public int Id {get; set;}
        public int ProductId {get;set;}
        public string ProductName {get; set;}
        public Product Product {get;set;}
        
        public decimal EstimatedPrice {get; set;} //Calculated from DistancePrice and TimePrice in Product
        public string CustomerId {get;set;}
        public Customer Customer {get;set;}

        public List<Product> Products { get; set; }
    }
}