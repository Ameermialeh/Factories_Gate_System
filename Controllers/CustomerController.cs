using FactoriesGateSystem.Models.DTOs.CustomerDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
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

                return Ok(customerDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:int}")]
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

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<CustomerDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCustomerName(string name)
        {
            try
            {
                var customerDto = await _customerRepo.GetCustomersAsync(c => c.Name.Contains(name));
                return Ok(customerDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerDTO customerDto)
        {
            if(customerDto.Name == null && customerDto.Address == null && customerDto.Phone == null)
                return BadRequest("All field (Name and Address and Phone) must be provided.");

            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var customer = await _customerRepo.AddCustomerAsync(customerDto, int.Parse(factoryId));
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500,  ex );
            }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerDTO dto)
        {
            if (id <= 0)
                return BadRequest("Invalid customer id.");
            if(dto.Name == null && dto.Address == null && dto.Phone == null)
                return BadRequest("At least one field (Name or Address or Phone) must be provided.");

            try
            {
                var customer = await _customerRepo.UpdateCustomerAsync(id, dto);
                if (customer == null)
                    return NotFound($"No Customer with id: {id}."); 

                return Ok(customer);
            } catch(Exception) { return StatusCode(500, "Internal server error");}
        }

        [HttpDelete("{id:int}")]
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
                var done = await _customerRepo.DeleteCustomerAsync(id);
                if (!done)
                    return NotFound($"No customer with id = {id}.");
               
                return Ok("Customer deleted successfully");
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
