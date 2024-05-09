using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsMVP.Model;

namespace WinFormsMVP.View
{
    public interface ICustomerListView
    {
        IList<Customer> CustomersList { get; set; }
        Presenter.CustomerPresenter Presenter { get; set; }
    }
}
