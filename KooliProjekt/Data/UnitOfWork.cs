using KooliProjekt.Data.Repositories;
using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork
    {
        public IOrderRepository OrderRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        // Pluss kõik teised repositoryd

        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context, IOrderRepository orderRepository, IProductRepository productRepository, IInvoiceRepository invoiceRepository)
        {
            _context = context;

            OrderRepository = orderRepository;
            ProductRepository = productRepository;
            InvoiceRepository = invoiceRepository;
        }

        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
