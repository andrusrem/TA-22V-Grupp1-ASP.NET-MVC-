namespace KooliProjekt.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> List();
        Task<Customer> GetById(string id);
        Task Save(string id, Customer entity);
        Task Delete(string id);
        Task<Customer> GetByEmail(string email);
        Task<IList<LookupCustomer>> Lookup();
        Task Entry(string id, Customer customer);
        Task Add(Customer customer);
        bool Existance(string id);
    }
}