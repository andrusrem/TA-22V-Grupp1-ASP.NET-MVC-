namespace KooliProjekt.Data
{
    public class Invoice
    {
        public int Id {get; set;}
        public int ProductId {get;set;}
        public Product Product {get;set;}
        public DateTime WhenTaken {get; set;}
        public DateTime GivenBack {get; set;}
        public decimal DistanceDriven {get; set;}
        public decimal TotalPrice {get; set;} //Time and Distance
        public DateTime PayBy {get; set;}
        public bool PayStatus {get; set;}
        public int CustomerId {get;set;}
        public Customer Customer {get;set;}
    }
}