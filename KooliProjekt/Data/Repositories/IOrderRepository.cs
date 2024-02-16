namespace KooliProjekt.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<PagedResult<Order>> List(int page, int pageSize);
        Task<List<Order>> GetAllOrders();
        Task<List<Order>> GetCustomerOrders(string email);
        Task<Order> GetById(int id);
        Task Save(Order list);
        Task Delete(int? id);
        bool Existance(int Id);
        Task Entry(Order order);
        Task Add(Order order);
    }
}