using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
 {
     public class CustomerRepository : ICustomerRepository
     {
        protected ApplicationDbContext Context { get; }

        public CustomerRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public async Task<List<Customer>> List()
        {
            var result = await Context.Customers.AsQueryable().ToListAsync();

            return result;
        }


        public async Task<Customer> GetById(string id)
        {
            var result = await Context.Set<Customer>().FindAsync(id);

            return result;
        }

        public async Task Save(string id, Customer entity)
        {
            var customerDb = Context.Customers.FirstOrDefault(x => x.Id == id);
            customerDb.Name = entity.Name;
            customerDb.UserName = entity.UserName;
            customerDb.Phone = entity.Phone;
            customerDb.Address = entity.Address;
            customerDb.City = entity.City;
            customerDb.Postcode = entity.Postcode;
            customerDb.Country = entity.Country;
            if (string.IsNullOrEmpty(entity.Id))
            {
                await Context.AddAsync(entity);
            }
            else
            {
                Context.Update(entity);
            }
            await Context.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var entity = await Context.Set<Customer>().FindAsync(id);
            if (entity != null)
            {
                Context.Set<Customer>().Remove(entity);
            }
            await Context.SaveChangesAsync();
        }
        public async Task<Customer> GetByEmail(string email)
        {
            var customer = await Context.Customers
                .FirstOrDefaultAsync(m => m.UserName == email);
            return customer;
        }
        public async Task<IList<LookupCustomer>> Lookup()
        {

            return await Context.Customers
                .OrderBy(c => c.Name)
                .Select(c => new LookupCustomer
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToListAsync();
        }
        public async Task Entry(string id, Customer customer)
        {
            Context.Entry(customer).State = EntityState.Modified;

            await Context.SaveChangesAsync();

        }
        public async Task Add(Customer customer)
        {
            Context.Customers.Add(customer);


            await Context.SaveChangesAsync();

        }
        public bool Existance(string id)
        {
            return Context.Customers.Any(e => e.Id == id);
        }
    }
 }