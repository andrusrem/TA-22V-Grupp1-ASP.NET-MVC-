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
        private string _name;
        private string _phone;
        private string _address;
        private string _city;
        private string _country;
        private string _email;
        private string _postCode;
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
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        public string Address {
            get { return _address; }
            set
            {
                _address = value;
                NotifyPropertyChanged(nameof(Address));
            }
        }

        public string Phone {
            get { return _phone; }
            set
            {
                _phone = value;
                NotifyPropertyChanged(nameof(Phone));
            }
        }
        public string Email {
            get { return _email; }
            set
            {
                _email = value;
                NotifyPropertyChanged(nameof(Email));
            }
        }
        public string City {
            get { return _city; }
            set
            {
                _city = value;
                NotifyPropertyChanged(nameof(City));
            }
        }
        public string Postcode {
            get { return _postCode; }
            set
            {
                _postCode = value;
                NotifyPropertyChanged(nameof(Postcode));
            }
        }
        public string Country {
            get { return _country; }
            set
            {
                _country = value;
                NotifyPropertyChanged(nameof(Country));
            }
        }

       
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
