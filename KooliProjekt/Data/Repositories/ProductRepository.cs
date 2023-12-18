using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;
namespace KooliProjekt.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
         public ProductRepository(ApplicationDbContext context) : base(context)
        {
            
        }
         public async Task<PagedResult<Product>> List(int page, int pageSize)
        {
            var result = await Context.Products.GetPagedAsync(page, pageSize);
            return result;

        }

        public async Task<Product> GetById(int Id)
        {
            var product = await Context.Products
                .FirstOrDefaultAsync(m => m.Id == Id);
            return product;
        }

        public async Task Save(Product product)
        {
            if (product.Id == 0)
            {
                Context.Add(product);
            }
            else
            {
                Context.Update(product);
            }

            await Context.SaveChangesAsync();
                    
        }

        public async Task Delete(int Id)
        {
            var product = await Context.Products.FindAsync(Id);
            if(product != null)
            {
                Context.Products.Remove(product);
            }

            await Context.SaveChangesAsync();

        }
        public async Task<IList<LookupItem>> Lookup()
        {
            
            return await Context.Products
                .OrderBy(p => p.Brand)
                .ThenBy(p => p.Model)
                .ThenBy(p => p.CarNum)
                .Select(p => new LookupItem{
                    Id = p.Id,
                    Name = p.Brand + " " + p.Model + " " + p.CarNum
                }).ToListAsync();
        }

        public bool Existance(int Id)
        {
            return Context.Products.Any(e => e.Id == Id);
        }
    }
}