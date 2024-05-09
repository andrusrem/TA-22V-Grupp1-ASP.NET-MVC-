using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;


namespace KooliProjekt.Controllers
{
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IImageService _imageService;

        public string Role { get; private set; }

        public OrderController(IImageService imageService, IOrderService orderService, IProductService productService, ICustomerService customerService)
        {
            _orderService = orderService;
            _productService = productService;
            _customerService = customerService;
            _imageService = imageService;
        }

        // GET: Order
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int page = 1)
        {
            
            return View(await _orderService.List(page, 2));
            
            //return View(await applicationDbContext.ToListAsync());
        }
        public IActionResult Image(int id)
        {
            return File(_imageService.ReadImage(id), "image/jpeg");
            
        }

        public async Task<IActionResult> Myorders()
        {


           string loggedInUsername = User.Identity.Name; // Get the logged-in username


            // Retrieve the orders and the products for the logged-in user
            List<Order> orders = await _orderService.GetCustomerOrders(loggedInUsername);

            List<Myorders> myorders = orders.Select(o => new Myorders
            {
                Id = o.Id,
                ProductName = o.Product.CarName,
                ProductEstimatedPrice = o.ProductEstimatedPrice,
                ProductId = o.ProductId,
                CustomerId = o.Customer.Name,
                WhenTaken = o.WhenTaken.Value
            }).ToList();
            // Map the orders and products to the ViewModel
            
            return View(myorders);
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue || !_orderService.Existance(id.Value))
            {
                return NotFound();
            }

            var order = await _orderService.GetById(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([FromQuery] int? productId)
        {
            var r = new Order();
 
            if (!User.IsInRole("Admin"))
            { 
                var userId = User.Identity.Name;
                
                r.Customer = await _customerService.GetByEmail(userId);
                r.CustomerId = r.Customer.Id;
                r.ProductId = productId.Value;
                r.WhenTaken = DateTime.Now;
                r.Product = await _productService.GetById(productId.Value);
                r.ProductEstimatedPrice = r.Product.EstimatedPrice;
                ViewData["ProductId"] = r.Product.CarName;
                ViewData["CustomerId"] = r.Customer.Name;

                await _orderService.Save(r);
                return RedirectToAction(nameof(Myorders));
            }
            else
            {
                
                if (productId.HasValue)
                r.ProductId = productId.Value;
            
            

                ViewData["ProductId"] = new SelectList(await _productService.Lookup(), "Id", "Name");
                ViewData["CustomerId"] = new SelectList(await _customerService.Lookup(), "Id", "Name");
            
                return View(r);
            }
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,EstimatedPrice,CustomerId")] Order order)
        {
            
            if (ModelState.IsValid)
            {
                await _orderService.Save(order);
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductId"] = new SelectList(await _productService.Lookup(), "Id", "Name", order.ProductId);
            ViewData["CustomerId"] = new SelectList(await _customerService.Lookup(), "Id", "Name", order.CustomerId);
            return View(order);
        }
        

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue || !_orderService.Existance(id.Value))
            {
                return NotFound();
            }

            var order = await _orderService.GetById(id.Value);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(await _productService.Lookup(), "Id", "Name", order.ProductId);
            ViewData["CustomerId"] = new SelectList(await _customerService.Lookup(), "Id", "Name", order.CustomerId);
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,EstimatedPrice,CustomerId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.Save(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_orderService.Existance(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(await _productService.Lookup(), "Id", "Name", order.ProductId);
            ViewData["CustomerId"] = new SelectList(await _customerService.Lookup(), "Id", "Name", order.CustomerId);
            return View(order);
        }

        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || !_orderService.Existance(id.Value))
            {
                return NotFound();
            }
            
            var order = await _orderService.GetById(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
