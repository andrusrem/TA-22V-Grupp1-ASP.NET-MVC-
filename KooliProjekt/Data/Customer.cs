using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data
{

    [ExcludeFromCodeCoverage]
    
    public class Customer : IdentityUser
    {
        
        public string Name {get; set;}
        public string Phone {get; set;}
        public string Address {get; set;}
        public string City {get; set;}
        public string Postcode {get; set;}
        public string Country {get; set;}
        [JsonIgnore]
        public IList<Invoice> Invoices {get; set;}
        [JsonIgnore]
        public IList<Order> Orders {get; set;}

        
        public Customer()
        {
            Invoices = new List<Invoice>();
            Orders = new List<Order>();
        }

    }
}