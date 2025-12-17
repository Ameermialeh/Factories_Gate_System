using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.DTOs.CustomerDTOs;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly CustomerRepo _customerRepo;
        public CustomerController(CustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [HttpGet("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            try
            {
                var customers = _customerRepo.GetCustomers();
                if (customers == null)
                    return NotFound("There is no customers Found.");

                var customerDto = customers.Select(c => new CustomerDTO()
                {
                    ID = c.CustomerId,
                    Name = c.CustomerName,
                    Address = c.Address,
                    Phone = c.PhoneNumber,
                    CurrentBalance = c.CurrentBalance,
                });
                return Ok(customerDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetCustomerByID/{id}")]
        public IActionResult GetCustomerByID(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid customer id.");
            try
            {
                var customer = _customerRepo.GetCustomerById(id);
                if (customer == null)
                    return NotFound($"No customer with id = {id}.");

                var customerDto = new CustomerDTO()
                {
                    ID = customer.CustomerId,
                    Name = customer.CustomerName,
                    Address = customer.Address,
                    Phone = customer.PhoneNumber,
                    CurrentBalance = customer.CurrentBalance,
                };
                return Ok(customerDto);
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateCustomer")]
        public IActionResult CreateCustomer([FromBody]CustomerDTO customerDto)
        {
            try
            {
                var customer = _customerRepo.AddCustomer(customerDto);
                return Ok(customer);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateCustomer/{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] UpdateCustomerDTO customerDto)
        {
            if (id <= 0 || customerDto == null)
                return BadRequest("Invalid customer data.");
            try
            {
                var customer = _customerRepo.UpdateCustomer(id, customerDto);
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

        [HttpDelete("DeleteCustomer/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid customer id.");
            try
            {
                var customer = _customerRepo.DeleteCustomer(id);
                if (customer == null)
                    return NotFound($"No customer with id = {id}.");

                var customerDto = new DeleteCustomerDTO()
                {
                    ID = customer.CustomerId,
                    Name = customer.CustomerName,
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
