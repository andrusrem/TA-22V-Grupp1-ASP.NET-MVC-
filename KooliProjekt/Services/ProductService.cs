using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc;
using KooliProjekt.Controllers;
using System.ComponentModel;

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

        public async Task<Product> GetById(int Id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == Id);
            return product;
        }

        public async Task Save(Product product)
        {
            if (product.Id == 0)
            {
                _context.Add(product);
            }
            else
            {
                _context.Update(product);
            }

            await _context.SaveChangesAsync();
                    
        }

        public async Task Delete(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if(product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();

        }

        public bool Existance(int Id)
        {
            return _context.Products.Any(e => e.Id == Id);
        }

    }
}