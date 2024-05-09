using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
namespace KooliProjekt.Data.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public override async Task<PagedResult<Invoice>> List(int page, int pageSize)
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

        public override async Task<Invoice> GetById(int id)
        {
            var invoice = await Context.Invoices
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            return invoice;
        }

        public override async Task Save(Invoice invoice)
        {
            await base.Save(invoice);
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
        public async Task Add(Invoice invoice)
        {
            Context.Invoices.Add(invoice);
            await Context.SaveChangesAsync();
        }
        public async Task Entry(Invoice invoice)
        {
            Context.Entry(invoice).State = EntityState.Modified;
            await Context.SaveChangesAsync();
        }
        public async Task<List<Invoice>> GetAllInvoices()
        {
            return await Context.Invoices.ToListAsync();
        }
    }


}
