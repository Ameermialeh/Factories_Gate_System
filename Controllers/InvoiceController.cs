using FactoriesGateSystem.Models.DTOs.InvoiceDTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InvoiceDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInvoices([FromQuery] int? orderId)
        {
            if (orderId == null)
            {
                var invoice = await _invoiceService.GetAllInvoicesAsync();
                return Ok(invoice);
            }
            var filtered = await _invoiceService.GetAllInvoicesWithFilterAsync(orderId.Value);
            return Ok(filtered);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(InvoiceDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid invoice id.");

            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            return Ok(invoice);
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(InvoiceDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] UpdateInvoiceDTO dto)
        {
            if (id <= 0) 
                return BadRequest("Invalid Invoice id");

            if(dto.Date == null && dto.Total == null)
                return BadRequest("At least one field (Date or Total) must be provided.");

            if(dto.Total < 0 )
                return BadRequest("Total cannot be negative.");

            var invoice = await _invoiceService.UpdateInvoiceAsync(id, dto);
            return Ok(invoice);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteInvoice(int id) {
            if (id <= 0)
                return BadRequest("Invalid invoice id.");
          
            await _invoiceService.DeleteIvnoiceAsync(id);
            return Ok($"Invoice with {id} deleted Successfully");
        }
    }
}
