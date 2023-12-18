namespace KooliProjekt.Data.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetById(int id);
        Task Save(Customer list);
    }
}