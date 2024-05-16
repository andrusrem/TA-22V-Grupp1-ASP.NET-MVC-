using WinFormsMVP.Presenter;
using WinFormsMVP.Model;
using WinFormsMVP.View;
using Moq;

namespace WinFormsMVP.UnitTests
{
    public class UnitTest1
    {
        private readonly IList<Customer> stubCustomerList = new List<Customer> {
                new Customer {Id = "1", Email = "a@a.com", Name = "Jack", Address = "Nowhere, TX 1023", Phone = "123-456"},
                new Customer {Id = "2", Email = "aa@a.com", Name = "Jill", Address = "Nowhere, AZ 1026", Phone = "124-456"},
                new Customer {Id = "3", Email = "aaa@a.com", Name = "Sam", Address = "Nowhere, UT 1005", Phone = "125-456"}
        };

        private readonly Mock<ICustomerView> mockCustomerView;
        private readonly Mock<ICustomerRepository> mockCustomerRepository;
        private readonly CustomerPresenter presenter;

        public UnitTest1()
        {
            mockCustomerView = new Mock<ICustomerView>();
            mockCustomerRepository = new Mock<ICustomerRepository>();

            presenter = new CustomerPresenter(mockCustomerView.Object, mockCustomerRepository.Object);
        }

        [Fact]
        public void Presenter_constructor_ShouldFillViewCustomerList()
        {
            var mockView = Mock.Get(mockCustomerView.Object);
            var customerNames = from customer in stubCustomerList select customer.Email;
            var customerList = from customer in mockCustomerView.Object.CustomerList select customer.Email;
            Assert.Equal(customerList, customerNames);
        }

        [Fact]
        public void Presenter_UpdateCustomerView_ShouldPopulateViewWithRightCustomer()
        {
            //Arrange
            var mockRepo = Mock.Get(mockCustomerRepository.Object);
            var mockView = Mock.Get(mockCustomerView.Object);
            var customer = stubCustomerList[0];
            var customerTask = mockRepo.Setup(repository => repository.GetCustomerApi("1")).ReturnsAsync(customer);
            mockView.SetupSet(mock => mock.CustomerName = customer.Email).Verifiable();
            mockView.SetupSet(mock => mock.Address = customer.Address).Verifiable();
            mockView.SetupSet(mock => mock.Phone = customer.Phone).Verifiable();
            //Act
            presenter.UpdateCustomerView("1");

            //Assert

            mockCustomerRepository.VerifyAll();
            mockCustomerView.VerifyAll();
        }

    }
}