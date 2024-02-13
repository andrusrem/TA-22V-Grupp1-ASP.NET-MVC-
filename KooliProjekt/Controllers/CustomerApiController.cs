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
    public class CustomerApiController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerApiController(ICustomerService customerService)
        {

            _customerService = customerService;
        }

        // GET: api/CustomerApi
        [HttpGet]
        public async Task<ActionResult<IList<Customer>>> GetCustomers()
        {
            return await _customerService.GetCustomerAsync();
        }

        // GET: api/CustomerApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(string id)
        {
            var customer = await _customerService.GetById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/CustomerApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(string id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            try
            {
                await _customerService.Entry(id, customer);
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

            return NoContent();
        }

        // POST: api/CustomerApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            try
            {
                await _customerService.Add(customer);
            }
            catch (DbUpdateException)
            {
                if (_customerService.Existance(customer.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // DELETE: api/CustomerApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var customer = await _customerService.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }

            await _customerService.Delete(id);

            return NoContent();
        }

        
    }
}
