using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjektMVVP.ApiClient
{
    public class Customer : INotifyPropertyChanged
    {
        private string _id;
        private string _title;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public string Address { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }

       
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
