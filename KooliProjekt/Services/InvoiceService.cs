using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public class InvoiceService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Invoice>> List(int page, int pageSize)
        {
             var result = await _context.Invoices
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .GetPagedAsync(page, pageSize);
            return result;
        }
        public async Task<Invoice> GetById(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Product)
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            return invoice;
        }

        public async Task Save(Invoice invoice)
        {
            if(invoice.Id == 0)
            {
                _context.Add(invoice);
            }
            else
            {
                _context.Update(invoice);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Invoice> FindId(int id)
        {
            return await _context.Invoices.FindAsync(id);
        }

        public async Task Delete(int? id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }
            
            await _context.SaveChangesAsync();
        }

        public bool Existance(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }

        internal Task<string> FindId(int? id)
        {
            throw new NotImplementedException();
        }
    }
}