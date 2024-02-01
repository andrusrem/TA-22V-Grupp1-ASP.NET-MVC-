using System.Diagnostics.CodeAnalysis;
using KooliProjekt.Data.Repositories;

namespace KooliProjekt.Data
{
    
    public interface IUnitOfWork
    {
        [ExcludeFromCodeCoverage]
        public IOrderRepository OrderRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IInvoiceRepository InvoiceRepository { get; }
        // Pluss kõik teised repositoryd

        Task BeginTransaction();
        Task Commit();
        Task Rollback();
    }
}
