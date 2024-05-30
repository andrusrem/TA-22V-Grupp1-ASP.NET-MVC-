using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using KooliProjektMVVP.ApiClient;
using KooliProjektMVVP;

namespace KooliProjektMVVM.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<ICustomerApiClient> _mockClient;
        private readonly MainWindowViewModel _model; 

        public MainWindowViewModelTests()
        {
            _mockClient = new Mock<ICustomerApiClient>();
            _model = new MainWindowViewModel(_mockClient.Object);
        }

        [Fact]
        public void ViewModel_loads_data_when_created()
        {
            // Arrange
            List<Customer> customers = new List<Customer>();
            _mockClient.Setup(c => c.List())
                          .Returns(customers)
                          .Verifiable();

            // Act
            new MainWindowViewModel(_mockClient.Object);

            // Assert
            _mockClient.VerifyAll();
        }
        [Fact]
        public void SelectedItem_returns_correct_item()
        {
            // Arrange
            var item = new Customer();

            // Act
            _model.SelectedItem = item;

            // Assert
            Assert.Equal(item, _model.SelectedItem);
        }
    }
}
