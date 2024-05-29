using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjektMVVP.ApiClient
{
    public interface ICustomerApiClient
    {
        List<Customer> List();
        Task<List<Customer>> ListAsync();
        Customer Get(string id);
        Task<Customer> GetAsync(string id);
        void Save(Customer customer);
        Task SaveAsync(Customer customer);
        void Delete(string id);
        Task DeleteAsync(string id);
    }
}
