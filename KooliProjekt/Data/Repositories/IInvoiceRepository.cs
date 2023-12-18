namespace KooliProjekt.Data.Repositories
{
    public interface IInvoiceRepository
    {
        Task<PagedResult<Invoice>> List(int page, int pageSize);
        Task<Invoice> GetById(int id);
        Task Save(Invoice invoice);
        Task<Invoice> FindId(int id);
        Task Delete(int? id);
        bool Existance(int id);
        
    }
}