using KooliProjekt.Data;
using KooliProjekt;
namespace KooliProjekt.Services
{
    public interface IProductService
    {
        Task<PagedResult<Product>> List(int page, int pageSize);
        Task<Product> GetById(int Id);
        Task Save(Product product);
        Task Delete(int Id);
        Task<IList<LookupItem>> Lookup();
        bool Existance(int Id);
    }
}