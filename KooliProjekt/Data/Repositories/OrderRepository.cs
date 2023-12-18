using KooliProjekt;
using Microsoft.EntityFrameworkCore;
namespace KooliProjekt.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
       
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public async Task<PagedResult<Order>> List(int page, int pageSize)
        {
            var result = await Context.Orders
                .Include(o => o.Product)
                .Include(o => o.Customer)
                .GetPagedAsync(page, pageSize);
            return result;

        }
        

        public async Task<Order> GetById(int id)
        {
            var order = await Context.Orders
                .Include(o => o.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            return order;
        }

        public async Task Save(Order order)
        {
            if(order.Id == 0)
            {
                Context.Add(order);
            }
            else
            {
                Context.Update(order);
            }
            await Context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            var order = await Context.Orders.FindAsync(id);

            if(order != null)
            {
                Context.Orders.Remove(order);
            }
            await Context.SaveChangesAsync();
        }
        // Queriyes

        public bool Existance(int Id)
        {
            return Context.Orders.Any(e => e.Id == Id);
        }

        
    }
}