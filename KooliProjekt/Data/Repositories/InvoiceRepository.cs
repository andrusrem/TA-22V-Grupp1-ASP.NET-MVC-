using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
namespace KooliProjekt.Data.Repositories
{
    public class InvoiceRepository : BaseRepository<Product>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<PagedResult<Invoice>> List(int page, int pageSize)
        {
             var result = await Context.Invoices
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .GetPagedAsync(page, pageSize);
            return result;
        }

        public async Task<List<Invoice>> GetCustomerInvoices(string email)
        {
            var result = await Context.Invoices
                .Where(o => o.Customer.Email == email)
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .ToListAsync();
            return result;
        }

        public async Task<Invoice> GetById(int id)
        {
            var invoice = await Context.Invoices
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            return invoice;
        }

        public async Task Save(Invoice invoice)
        {
            if(invoice.Id == 0)
            {
                Context.Add(invoice);
            }
            else
            {
                Context.Update(invoice);
            }
            await Context.SaveChangesAsync();
        }

        public async Task<Invoice> FindId(int id)
        {
            return await Context.Invoices.FindAsync(id);
        }

        public async Task Delete(int? id)
        {
            var invoice = await Context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                Context.Invoices.Remove(invoice);
            }
            
            await Context.SaveChangesAsync();
        }

        public bool Existance(int id)
        {
            return Context.Invoices.Any(e => e.Id == id);
        }

        internal Task<string> FindId(int? id)
        {
            throw new NotImplementedException();
        }
    }


}
