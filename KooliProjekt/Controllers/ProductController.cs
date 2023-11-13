using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt;

namespace KooliProjekt.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _context.Products.GetPagedAsync(page, 5);
            return View(result);
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

            var path = Url.Content("/Users/andrusremets/TA-22V-Grupp1/KooliProjekt/Images/" + product.Id + ".jpg");


            using(var stream = image.OpenReadStream())
            using(var fileStream = new FileStream(path, FileMode.CreateNew))
            {
                await stream.CopyToAsync(fileStream);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Image(int id)
        {
            var path = "/Users/andrusremets/TA-22V-Grupp1/KooliProjekt/Images/" + id + ".jpg";
            var stream = System.IO.File.OpenRead(path);
            return File(stream, "image/jpeg");
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
                var path = Url.Content("/Users/andrusremets/TA-22V-Grupp1/KooliProjekt/Images/" + product.Id + ".jpg");

                if (image != null) 
                {
                    using(var stream = image.OpenReadStream())
                    using(var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await stream.CopyToAsync(fileStream);
                        
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
