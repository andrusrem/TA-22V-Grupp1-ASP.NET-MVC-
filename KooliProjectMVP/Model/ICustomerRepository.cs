using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsMVP.Model
{
    public interface ICustomerRepository
    {
        Task<IList<Customer>> List();
        Task<Customer> Get(string id);

        Task<List<Customer>> GetCustomers();
        Task<Customer> GetCustomerApi(string id);
    }
}
