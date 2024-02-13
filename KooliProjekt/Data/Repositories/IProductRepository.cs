namespace KooliProjekt.Data.Repositories
{
     public interface IProductRepository
    {
        Task<PagedResult<Product>> List(int page, int pageSize);
        Task<Product> GetById(int Id);
        Task Save(Product product);
        Task Delete(int Id);
        Task<IList<LookupItem>> Lookup();
        bool Existance(int Id);
        Task Add(Product product);
        Task Entry(Product product);
        Task<List<Product>> GetAllProducts();
    }
}