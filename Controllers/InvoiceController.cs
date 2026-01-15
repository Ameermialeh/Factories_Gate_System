using FactoriesGateSystem.DTOs.ExpenseDTOs;
using FactoriesGateSystem.DTOs.InvoiceDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FactoriesGateSystem.DTOs.InvoiceDTOs.UpdateInvoiceDTO;

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
        [ProducesResponseType(typeof(InvoiceDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllInvoices()
        {
            try
            {
                var invoice = await _invoiceRepo.GetAllInvoicesAsync();
                if (invoice == null) { return NotFound("Invoices not Found!");}
                return Ok(invoice);

            }catch(Exception) { return StatusCode(500, "Internal server error"); }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetInvoiceById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid invoice id.");
            try
            {
                var invoice = await _invoiceRepo.GetInvoiceByIdAsync(id);
                if (invoice == null) { return NotFound($"No invoice with id = {id}. "); }
                return Ok(invoice);
            }
            catch (Exception) { return StatusCode(500, "Internal server error");  }
        }


        [HttpPut("UpdateDate")]

        public async Task<IActionResult> UpdateInvoiceDate([FromBody] UpdateInvoiceDateDTO dto)
        {
            if (!ModelState.IsValid || dto.id <= 0) return BadRequest("Invalid input data");

            try
            {
                var invoice = await _invoiceRepo.UpdateInvoiceDateAsync(dto);
                if (invoice == null) { return NotFound($"No invoice with id = {dto.id}. "); }

                return Ok(invoice);
            }
            catch { return StatusCode(500, "Internal server error");  }
        }

        [HttpPut("UpdateTotal")]

        public async Task<IActionResult> UpdateInvoiceTotal([FromBody] UpdateInvoiceTotalDTO dto)
        {
            if (dto.Total < 0  || dto.id <= 0) return BadRequest("Invalid input data");

            try
            {
                var invoice = await _invoiceRepo.UpdateInvoiceTotalAsync(dto);
                if (invoice == null) { return NotFound($"No invoice with id = {dto.id}. "); }

                return Ok(invoice);
            }
            catch { return StatusCode(500, "Internal server error"); }
        }


        [HttpDelete("id")]
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
