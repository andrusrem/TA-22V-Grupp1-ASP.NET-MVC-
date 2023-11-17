using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ImageService _imageService;

        private readonly ProductService _productService;

        public ProductController(ApplicationDbContext context, ImageService imageService, ProductService productService)
        {
            _context = context;
            _imageService = imageService;
            _productService = productService;
        }



        // GET: Product
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _productService.List(page, 5));
        }


        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Brand,Model,Manufacturer,CarNum,CarType,DistancePrice,TimePrice")] Product product, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {       
                return View(product);
            }
            
            _context.Add(product);
            await _context.SaveChangesAsync();


            using(var stream = image.OpenReadStream())
            {
                await _imageService.WriteImage(product.Id, stream);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Image(int id)
        {
            return File(_imageService.ReadImage(id), "image/jpeg");
            
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,Model,Manufacturer,CarNum,CarType,DistancePrice,TimePrice")] Product product, IFormFile image)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                

                if (image != null) 
                {
                    
                    using(var stream = image.OpenReadStream())
                    {
                        await _imageService.UpdateImage(product.Id, stream);
                    }
                }
                else if (image == null)
                {
                    using(var stream = image.OpenReadStream())
                    {
                        await _imageService.WriteImage(product.Id, stream);
                    }
                }
 
                
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        
        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.Id == id);
        }
    }
}
