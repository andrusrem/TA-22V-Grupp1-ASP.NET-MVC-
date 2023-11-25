using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using System.Threading.Tasks;
using System.Data;

namespace KooliProjekt.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomerAsync()
        {
            
            var query = _context.Customers.AsQueryable();
            return await query.ToListAsync();
        }
        public async Task<Customer> GetById(string id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            return customer;
        }

        public async Task<IList<LookupCustomer>> Lookup()
        {
            
            return await _context.Customers
                .OrderBy(c => c.Name)
                .Select(c => new LookupCustomer{
                    Id = c.Id,
                    Name = c.Name,
                }).ToListAsync();
        }

        public async Task Save(string id, Customer customer)
        {
            var customerDb = _context.Customers.FirstOrDefault(x => x.Id ==id);
            customerDb.Name = customer.Name;
            customerDb.UserName = customer.UserName;
            customerDb.Phone = customer.Phone;
            customerDb.Address = customer.Address;
            customerDb.City = customer.City;
            customerDb.Postcode = customer.Postcode;
            customerDb.Country = customer.Country;
            
            if(customer.Id != null)
            {
                _context.Update(customerDb);
            }
            else
            {
                _context.Add(customerDb);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
        }
        public bool Existance(string id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}