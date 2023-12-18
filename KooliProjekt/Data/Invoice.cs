using KooliProjekt;

namespace KooliProjekt.Data
{
    public class Invoice : Entity
    {
        public int Id {get; set;}
        public int ProductId {get;set;}
        public Product Product {get;set;}
        public DateTime? WhenTaken {get; set;}
        public DateTime? GivenBack {get; set;}
        public decimal DistanceDriven {get; set;}
        public decimal TotalPrice {get; set;} //Time and Distance
        public DateTime PayBy {get; set;}
        public bool PayStatus {get; set;}
        public string CustomerId {get;set;}
        public Customer Customer {get;set;}
        public int? OrderId {get; set;}
        public Order Order {get; set;}
    }
}