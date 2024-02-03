using KooliProjekt.Data;
using KooliProjekt;
namespace KooliProjekt.Services
{
    public interface IOrderService
    {
        Task<PagedResult<Order>> List(int page, int pageSize);
        Task<List<Order>> GetCustomerOrders(string email);
        Task<Order> GetById(int id);
        Task Save(Order order);
        Task Delete(int? id);
        bool Existance(int id);
        
    }
}