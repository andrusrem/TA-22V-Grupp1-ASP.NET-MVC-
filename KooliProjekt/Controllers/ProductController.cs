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
using Microsoft.AspNetCore.Authorization;

namespace KooliProjekt.Controllers
{
    public class ProductController : Controller
    {
        

        private readonly IImageService _imageService;

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ProductController(IImageService imageService, IProductService productService, IOrderService orderService)
        {
            _imageService = imageService;
            _productService = productService;
            _orderService = orderService;
        }



        // GET: Product
        
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await _productService.List(page, 5));
        }


        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue || !_productService.Existance(id.Value))
            {
                return NotFound();
            }

            var product = await _productService.GetById(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = "Admin,User")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([Bind("Id,Brand,Model,Manufacturer,CarNum,CarType,DistancePrice,TimePrice")] Product product, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {       
                return View(product);
            }
            
            await _productService.Save(product);


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
            if (!id.HasValue || !_productService.Existance(id.Value))
            {
                return NotFound();
            }

            var product = await _productService.GetById(id.Value);
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
                    await _productService.Save(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_productService.Existance(product.Id))
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
 
                
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        
        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || !_productService.Existance(id.Value))
            {
                return NotFound();
            }

            var product = await _productService.GetById(id.Value);
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
            
            await _productService.Delete(id);
            return RedirectToAction(nameof(Index));
        }   
    }
}
