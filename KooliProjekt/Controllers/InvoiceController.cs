using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Authorization;

namespace KooliProjekt.Controllers
{
    public class InvoiceController : Controller
    {
        

        private readonly InvoiceService _invoiceService;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private readonly ApplicationDbContext _context;

        public InvoiceController(ApplicationDbContext context, InvoiceService invoiceService, ProductService productService, CustomerService customerService)
        {
            _invoiceService = invoiceService;
            _productService = productService;
            _customerService = customerService;
            _context = context;
        }

        // GET: Invoice
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _invoiceService.List(page, 5);
            return View(result);
            //return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult Myinvoices()
        {


           string loggedInUsername = User.Identity.Name; // Get the logged-in username


            // Retrieve the orders and the products for the logged-in user
            List<Invoice> invoices = _context.Invoices
                .Where(o => o.Customer.Email == loggedInUsername)
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .ToList();

            List<Myinvoice> myinvoices = invoices.Select(o => new Myinvoice
            {
                Id = o.Id,
                ProductName = o.Product.CarName,
                TotalPrice = o.TotalPrice,
                CustomerId = o.Customer.Name,
            }).ToList();
            // Map the orders and products to the ViewModel
            
            return View(myinvoices);
        }

        // GET: Invoice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue || !_invoiceService.Existance(id.Value))
            {
                return NotFound();
            }

            var invoice = await _invoiceService.GetById(id.Value);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoice/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ProductId"] = new SelectList(await _productService.Lookup(), "Id", "Name");
            ViewData["CustomerId"] = new SelectList(await _customerService.Lookup(), "Id", "Name");
            return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,WhenTaken,GivenBack,DistanceDriven,TotalPrice,PayBy,PayStatus,CustomerId")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                await _invoiceService.Save(invoice);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(await _productService.Lookup(), "Id", "Name", invoice.ProductId);
            ViewData["CustomerId"] = new SelectList(await _customerService.Lookup(), "Id", "Name", invoice.CustomerId);
            return View(invoice);
        }

        // GET: Invoice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue || !_invoiceService.Existance(id.Value))
            {
                return NotFound();
            }

            var invoice = await _invoiceService.GetById(id.Value);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(await _productService.Lookup(), "Id", "Name", invoice.ProductId);
            ViewData["CustomerId"] = new SelectList(await _customerService.Lookup(), "Id", "Name", invoice.CustomerId);
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,WhenTaken,GivenBack,DistanceDriven,TotalPrice,PayBy,PayStatus,CustomerId")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _invoiceService.Save(invoice);   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_invoiceService.Existance(id))
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
            ViewData["ProductId"] = new SelectList(await _productService.Lookup(), "Id", "Name", invoice.ProductId);
            ViewData["CustomerId"] = new SelectList(await _customerService.Lookup(), "Id", "Name", invoice.CustomerId);
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue || !_invoiceService.Existance(id.Value))
            {
                return NotFound();
            }

            var invoice = await _invoiceService.GetById(id.Value);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            await _invoiceService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
