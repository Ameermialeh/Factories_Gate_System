using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.DTOs.CustomerDTOs;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly CustomerRepo _customerRepo;
        public CustomerController(CustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customerDto = await _customerRepo.GetCustomersAsync();
                if (customerDto == null)
                    return NotFound("There is no customers Found.");

                return Ok(customerDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCustomerByID(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid customer id.");
            try
            {
                var customer =await _customerRepo.GetCustomerByIdAsync(id);
                if (customer == null)
                    return NotFound($"No customer with id = {id}.");

                var customerDto = new CustomerDTO()
                {
                    ID = customer.CustomerId,
                    Name = customer.Name,
                    Address = customer.Address,
                    Phone = customer.Phone,
                };
                return Ok(customerDto);
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerDTO customerDto)
        {
            try
            {
                var customer = await _customerRepo.AddCustomerAsync(customerDto);
                return Ok(customer);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateCustomerDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDTO customerDto)
        {
            if (id <= 0 || customerDto == null)
                return BadRequest("Invalid customer data.");
            try
            {
                var customer =await _customerRepo.UpdateCustomerAsync(id, customerDto);
                if (customer == null)
                {
                    return NotFound($"No Customer with id: {id}.");
                }

                return Ok(customer);
            }
            catch(Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteCustomerDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid customer id.");
            try
            {
                var customer = await _customerRepo.DeleteCustomerAsync(id);
                if (customer == null)
                    return NotFound($"No customer with id = {id}.");

                var customerDto = new DeleteCustomerDTO()
                {
                    ID = customer.CustomerId,
                    Name = customer.Name,
                };
                return Ok(customerDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
