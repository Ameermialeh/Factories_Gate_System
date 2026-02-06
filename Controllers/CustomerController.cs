using FactoriesGateSystem.Models.DTOs.CustomerDTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customerDto = await _customerService.GetAllCustomersAsync(); 
            return Ok(customerDto);
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

            var customerDto = await _customerService.GetCustomerByIdAsync(id);
            return Ok(customerDto);
        } 

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<CustomerDTO>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetCustomerName(string name)
        {
            var customers = await _customerService.GetCustomerNameAsync(name);
            return Ok(customers);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customer = await _customerService.CreateCustomerAsync(dto);

            return Ok(customer);
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

            var customer = await _customerService.UpdateCustomerAsync(id, dto);
            return Ok(customer);
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

            await _customerService.DeleteCustomerAsync(id);

            return Ok("Customer deleted successfully");          
        }
    }
}
