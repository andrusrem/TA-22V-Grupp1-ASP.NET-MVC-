using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Newtonsoft;

namespace WinFormsMVP.Model
{
    internal class CustomerXmlRepository : ICustomerRepository
    {
        private readonly string _xmlFilePath;
        private readonly XmlSerializer _serializer = new XmlSerializer(typeof(List<Customer>));
        private readonly Lazy<List<Customer>> _customers;
        private static HttpClient _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7136/api/CustomerApi/") };

        public CustomerXmlRepository(string fullPath)
        {
            
            _xmlFilePath = fullPath + @"\customers.xml";

            if (!File.Exists(_xmlFilePath))
                CreateCustomerXmlStub();

            _customers = new Lazy<List<Customer>>(() =>
            {
                using (var reader = new StreamReader(_xmlFilePath))
                {
                    return (List<Customer>)_serializer.Deserialize(reader);
                }
            });
        }

        private void CreateCustomerXmlStub()
        {
            var stubCustomerList = new List<Customer> {
                new Customer {Name = "Joe", Address = "Nowhere, TX 1023", Phone = "123-456"},
                new Customer {Name = "Jane", Address = "Nowhere, AZ 1026", Phone = "124-456"},
                new Customer {Name = "Steve", Address = "Nowhere, UT 1005", Phone = "125-456"}
            };
            SaveCustomerList(stubCustomerList);
        }

        private void SaveCustomerList(List<Customer> customers)
        {
            using (var writer = new StreamWriter(_xmlFilePath, false))
            {
                _serializer.Serialize(writer, customers);
            }
        }


        public async Task<IList<Customer>> List()
        {
            return await _httpClient.GetFromJsonAsync<List<Customer>>("lists");
        }

        public async Task<Customer> Get(string id)
        {
            return await _httpClient.GetFromJsonAsync<Customer>("lists/" + id);
        }

        public async Task<List<Customer>> GetCustomers()
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
        public async Task<Customer> GetCustomerApi(string id)
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
    }
}