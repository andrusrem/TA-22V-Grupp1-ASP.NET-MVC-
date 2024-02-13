using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductApiController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductApiController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/ProductApi
        [HttpGet]
        public async Task<ActionResult<IList<Product>>> GetProducts()
        {
            return await _productService.GetAllProducts();
        }

        // GET: api/ProductApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/ProductApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                await _productService.Entry(product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_productService.Existance(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            await _productService.Add(product);

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/ProductApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productService.Delete(id);
            

            return NoContent();
        }
    }
}
