using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Query;

namespace KooliProjekt.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Order>> List(int page, int pageSize)
        {
            var result = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Customer)
                .GetPagedAsync(page, pageSize);
            return result;

        }
        

        public async Task<Order> GetById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            return order;
        }

        public async Task Save(Order order)
        {
            if(order.Id == 0)
            {
                _context.Add(order);
            }
            else
            {
                _context.Update(order);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? id)
        {
            var order = await _context.Orders.FindAsync(id);

            if(order != null)
            {
                _context.Orders.Remove(order);
            }
            await _context.SaveChangesAsync();
        }

        public bool Existance(int Id)
        {
            return _context.Orders.Any(e => e.Id == Id);
        }

        
    }
}