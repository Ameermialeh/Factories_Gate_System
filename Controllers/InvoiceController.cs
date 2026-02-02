using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.InvoiceDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.Models.DTOs.InvoiceDTOs.UpdateInvoiceDTO;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class InvoiceController : Controller
    {
        private readonly InvoiceRepo _invoiceRepo;

        public InvoiceController(InvoiceRepo invoiceRepo)
        {
            _invoiceRepo = invoiceRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<InvoiceDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInvoices([FromQuery] int? orderId)
        {
            try
            {
                if (orderId == null)
                {
                    var invoice = await _invoiceRepo.GetAllInvoicesAsync();
                    return Ok(invoice);
                }
                var filtered = await _invoiceRepo.GetAllInvoicesAsync(i => i.OrderId == orderId);
                return Ok(filtered);
            }
            catch(Exception) { return StatusCode(500, "Internal server error"); }
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
            try
            {
                var invoice = await _invoiceRepo.GetInvoiceByIdAsync(id);
                if (invoice == null) { return NotFound($"No invoice with id = {id}. "); }
                var invoiceDto = new InvoiceDTO()
                {
                    Id = id,
                    Total = invoice.Total,
                    Date = invoice.Date,
                    OrderId = invoice.OrderId,

                };
                return Ok(invoiceDto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error");  }
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(InvoiceDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] UpdateInvoiceDTO dto)
        {
            if (id <= 0) return BadRequest("Invalid Invoice id");
            if(dto.Date == null && dto.Total == null)
                return BadRequest("At least one field (Date or Total) must be provided.");
            if(dto.Total < 0 )
                return BadRequest("Total cannot be negative.");
            try
            {
                var invoice = await _invoiceRepo.UpdateInvoiceAsync(id, dto);
                if (invoice == null) { return NotFound($"No invoice with id = {id}. "); }

                return Ok(invoice);
            }
            catch { return StatusCode(500, "Internal server error");  }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteInvoice(int id) {
            if (id <= 0)
                return BadRequest("Invalid invoice id.");
            try
            {
                var done = await _invoiceRepo.DeleteIvnoiceAsync(id);
                if (!done) { return NotFound($"No invoice with id = {id}. "); }

                return Ok($"Invoice with {id} deleted Successfully");

            } catch (Exception) { return StatusCode(500, "Internal server error"); }
        }
    }
}
