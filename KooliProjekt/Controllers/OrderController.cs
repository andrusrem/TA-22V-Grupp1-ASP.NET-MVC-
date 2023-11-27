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

        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;

        public string Role { get; private set; }

        public OrderController(OrderService orderService, ProductService productService, CustomerService customerService)
        {
            _orderService = orderService;
            _productService = productService;
            _customerService = customerService;
        }

        // GET: Order
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index(int page = 1)
        {
            
            return View(await _orderService.List(page, 2));
            
            //return View(await applicationDbContext.ToListAsync());
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
            var newOrder = new Order();
            if (productId.HasValue)
            newOrder.ProductId = productId.Value;
            

            ViewData["ProductId"] = new SelectList(await _productService.Lookup(), "Id", "Name");
            ViewData["CustomerId"] = new SelectList(await _customerService.Lookup(), "Id", "Name");
            return View(newOrder);
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
