using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using System.Threading.Tasks;
using System.Data;
using KooliProjekt.Data.Repositories;

namespace KooliProjekt.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;

        }

        public async Task<List<Customer>> GetCustomerAsync()
        {
            
            var list = await _customerRepository.List();
            return list;
        }
        public async Task<Customer> GetById(string id)
        {
            var customer = await _customerRepository.GetById(id);
            return customer;
        }
        public async Task<Customer> GetByEmail(string email) {
            var customer = await _customerRepository.GetByEmail(email);
            return customer;
        }


        public async Task<IList<LookupCustomer>> Lookup()
        {
            
            return await _customerRepository.Lookup(); 
        }

        public async Task Save(string id, Customer customer)
        {
            await _customerRepository.Save(id, customer);
        }

        public async Task Delete(string id)
        {
            await _customerRepository.Delete(id);
        }
        public bool Existance(string id)
        {
            return _customerRepository.Existance(id);
        }

        public async Task Add(Customer customer)
        {
            await _customerRepository.Add(customer);
            
        }
        public async Task Entry(string id, Customer customer)
        {
            await _customerRepository.Entry(id, customer);
            
        }
    }
}
