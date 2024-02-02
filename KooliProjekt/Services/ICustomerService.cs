using KooliProjekt.Data;
namespace KooliProjekt.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetCustomerAsync();
        Task<Customer> GetById(string id);
        Task<Customer> GetByEmail(string email);
        Task<IList<LookupCustomer>> Lookup();
        Task Save(string id, Customer customer);
        Task Delete(string id);
        bool Existance(string id);
    }
}
