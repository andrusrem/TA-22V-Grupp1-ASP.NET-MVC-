using KooliProjekt.Data;
namespace KooliProjekt.Services
{
    public interface IInvoiceService
    {
        Task<PagedResult<Invoice>> List(int page, int pageSize);
        Task<List<Invoice>> GetCustomerInvoices(string email);
        Task<Invoice> GetById(int id);
        Task Save(Invoice invoice);
        Task<Invoice> FindId(int id);
        Task Delete(int? id);
        bool Existance(int id);
        Task Add(Invoice invoice);
        Task Entry(Invoice invoice);
        Task<List<Invoice>> GetAllInvoices();
    }
}
