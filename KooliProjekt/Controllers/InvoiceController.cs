using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KooliProjekt.Data;
using KooliProjekt.Services;

namespace KooliProjekt.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly InvoiceService _invoiceService;

        public InvoiceController(ApplicationDbContext context, InvoiceService invoiceService)
        {
            _context = context;
            _invoiceService = invoiceService;
        }

        // GET: Invoice
        public async Task<IActionResult> Index(int page = 1)
        {
            var result = await _invoiceService.List(page, 5);
            return View(result);
            //return View(await applicationDbContext.ToListAsync());
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
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "CarName");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "CarName", invoice.ProductId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", invoice.CustomerId);
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "CarName", invoice.ProductId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", invoice.CustomerId);
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
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "CarName", invoice.ProductId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", invoice.CustomerId);
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
