using KooliProjekt.Data;
namespace KooliProjekt.Services
{
    public interface IInvoiceService
    {
        Task<PagedResult<Invoice>> List(int page, int pageSize);
        Task<Invoice> GetById(int id);
        Task Save(Invoice invoice);
        Task<Invoice> FindId(int id);
        Task Delete(int? id);
        bool Existance(int id);
        
    }
}