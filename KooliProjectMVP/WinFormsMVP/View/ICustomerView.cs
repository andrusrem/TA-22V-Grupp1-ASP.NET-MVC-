using System.Collections.Generic;
using WinFormsMVP.Model;

namespace WinFormsMVP.View
{
    public interface ICustomerView
    {
        IList<Customer> CustomerList { get; set; }

        int SelectedCustomer { get; set; }

        string CustomerName { get; set; }

        string Address { get; set; }

        string Phone { get; set; }

        Presenter.CustomerPresenter Presenter { set; }
    }
}