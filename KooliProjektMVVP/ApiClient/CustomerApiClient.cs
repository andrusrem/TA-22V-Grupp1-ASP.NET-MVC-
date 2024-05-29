using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjektMVVP.ApiClient
{
    public class CustomerApiClient : ICustomerApiClient, IDisposable
    {
        private HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7136/") };

        public List<Customer> List()
        {
            var task = Task.Run(async () => await ListAsync());
            task.Wait();

            return task.Result;
        }

        public async Task<List<Customer>> ListAsync()
        {
            HttpClient _httpClient = new HttpClient();
            var client = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7136/api/CustomerApi");
            client.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.SendAsync(client);
            Console.WriteLine(response.ToString());
            var responseData = await response.Content.ReadAsStringAsync();

            var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Customer>>(responseData);

            List<Customer> result = users;

            return result;
        }

        public Customer Get(string id)
        {
            var task = Task.Run(async () => await GetAsync(id));
            task.Wait();

            return task.Result;
        }

        public async Task<Customer> GetAsync(string id)
        {
            HttpClient _httpClient = new HttpClient();
            var client = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7136/api/CustomerApi");
            client.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _httpClient.SendAsync(client);
            Console.WriteLine(response.ToString());
            var responseData = await response.Content.ReadAsStringAsync();

            var users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Customer>>(responseData);
            var customer = users.Where(u => u.Id == id).FirstOrDefault();

            return customer;
        }

        public void Save(Customer customer)
        {
            var task = Task.Run(async () => await SaveAsync(customer));
            task.Wait();
        }

        public async Task SaveAsync(Customer customer)
        {
            var url = "Customer/";

            if (customer.Id == null)
            {
                await _httpClient.PostAsJsonAsync(url, customer);
            }
            else
            {
                await _httpClient.PutAsJsonAsync(url + customer.Id, customer);
            }
        }

        public void Delete(string id)
        {
            var task = Task.Run(async () => await DeleteAsync(id));
            task.Wait();
        }

        public async Task DeleteAsync(string id)
        {
            await _httpClient.DeleteAsync("Customer/Delete/" + id);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
