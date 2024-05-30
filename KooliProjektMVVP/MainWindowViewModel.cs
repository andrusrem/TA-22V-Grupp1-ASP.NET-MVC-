using System.Collections.ObjectModel;
using System.ComponentModel;
using KooliProjektMVVP.ApiClient;

namespace KooliProjektMVVP
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ICustomerApiClient _apiClient;
        private Customer _selectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Customer> Lists { get; set; }
        
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand NewCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }

        public MainWindowViewModel(ICustomerApiClient apiClient) 
        { 
            _apiClient = apiClient;

            Lists = new ObservableCollection<Customer>();

            SaveCommand = new RelayCommand(
                p => true,
                p => { _apiClient.Save(SelectedItem); UpdateLists(); }
            );

            NewCommand = new RelayCommand(
                p => true,
                p => SelectedItem = new Customer()
            );

            DeleteCommand = new RelayCommand(
                p => SelectedItem != null,
                p => { _apiClient.Delete(SelectedItem.Id); UpdateLists(); }
            );

            UpdateLists();
        }

        

        public Customer SelectedItem
        { 
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged(nameof(SelectedItem));
            }
        }

        private void UpdateLists()
        {
            Lists.Clear();

            var lists = _apiClient.List() ?? new List<Customer>();

            foreach (var item in lists)
            {
                Lists.Add(item);
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}