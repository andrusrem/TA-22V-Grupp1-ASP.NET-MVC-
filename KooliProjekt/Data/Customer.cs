using Microsoft.AspNetCore.Identity;

namespace KooliProjekt.Data
{

    public class Customer : IdentityUser
    {
        
        public string Name {get; set;}
        public string Phone {get; set;}
        public string Address {get; set;}
        public string City {get; set;}
        public string Postcode {get; set;}
        public string Country {get; set;}

        public IList<Invoice> Invoices {get; set;}

        public Customer()
        {
            Invoices = new List<Invoice>();
        }

    }
}