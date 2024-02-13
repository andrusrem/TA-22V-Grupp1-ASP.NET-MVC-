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
    public class InvoiceApiController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;


        public InvoiceApiController(IOrderService orderService, IInvoiceService invoiceService, IProductService productService, ICustomerService customerService)
        {
            _invoiceService = invoiceService;
            _productService = productService;
            _customerService = customerService;
            _orderService = orderService;
        }

        // GET: api/InvoiceApi
        [HttpGet]
        public async Task<ActionResult<IList<Invoice>>> GetInvoices()
        {
            return await _invoiceService.GetAllInvoices();
        }

        // GET: api/InvoiceApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _invoiceService.GetById(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/InvoiceApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            try
            {
                await _invoiceService.Entry(invoice);
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

            return NoContent();
        }

        // POST: api/InvoiceApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            await _invoiceService.Add(invoice);

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // DELETE: api/InvoiceApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _invoiceService.GetById(id);
            if (invoice == null)
            {
                return NotFound();
            }

            
            await _invoiceService.Delete(id);

            return NoContent();
        }
    }
}
