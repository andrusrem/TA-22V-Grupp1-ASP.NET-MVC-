using WinFormsMVP.View;
using WinFormsMVP.Model;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System;
using System.Threading.Tasks;

namespace WinFormsMVP.Presenter
{
    public class CustomerPresenter
    {
        private readonly ICustomerView _view;
        private readonly ICustomerRepository _repository;

        public CustomerPresenter(ICustomerView view, ICustomerRepository repository)
        {
            _view = view;
            view.Presenter = this;
            _repository = repository;

            UpdateCustomerListView();
            
        }

        private void UpdateCustomerListView()
        {
            var task = Task.Run(async () => await _repository.GetCustomers());
            task.Wait();
            var list = task.Result;
            int selectedCustomer = _view.SelectedCustomer >= 0 ? _view.SelectedCustomer : 0;
            _view.CustomerList = list;
            _view.SelectedCustomer = selectedCustomer;
            

        }

        public void UpdateCustomerView(string p)
        {
            
            var customerTask = Task.Run(async () => await _repository.GetCustomerApi(p));
            customerTask.Wait();
            var customer = customerTask.Result;

            
            _view.CustomerName = customer.Email;
            _view.Address = customer.Address;
            _view.Phone = customer.Phone;
            
            
        }
    }
}