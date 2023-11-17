using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Product>> List(int page, int pageSize)
        {
            var result = await _context.Products.GetPagedAsync(page, pageSize);
            return result;

        }
    }
}