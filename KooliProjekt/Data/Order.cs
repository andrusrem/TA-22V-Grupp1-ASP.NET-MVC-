using KooliProjekt.Data;

namespace KooliProjekt
{
    public class Order
    {
        public int Id {get; set;}
        public int ProductId {get;set;}
        public Product Product {get;set;}
        public decimal EstimatedPrice {get; set;} //Calculated from DistancePrice and TimePrice in Product
        public int CustomerId {get;set;}
        public Customer Customer;
    }
}