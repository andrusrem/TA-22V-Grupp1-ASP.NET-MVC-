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
    
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: Customer
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
              return View(await _customerService.GetCustomerAsync());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
        // GET: Customer/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || !_customerService.Existance(id))
            {
                return NotFound();
            }

            var customer = await _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        
        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || !_customerService.Existance(id))
            {
                return NotFound();
            }

            var customer = await _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    await _customerService.Save(id, customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_customerService.Existance(id))
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
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || !_customerService.Existance(id))
            {
                return NotFound();
            }

            var customer = await _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _customerService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        
    }
}
