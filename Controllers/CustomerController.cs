using FactoriesGateSystem.DTOs;
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
            var customers = _customerRepo.GetCustomers();
            if(customers == null)
            {
                return BadRequest("There is no customers here. Add new customer");
            }
            var customerDto = customers.Select(c => new CustomerDTO()
            {
                ID = c.CustomerId,
                Name = c.CustomerName,
                Address = c.Address,
                Phone = c.PhoneNumber
            });
            return Ok(customerDto);
        }

        [HttpGet("GetCustomerByID/{id}")]
        public IActionResult GetCustomerByID(int id)
        {
            var customer = _customerRepo.GetCustomerById(id);
            if(customer == null)
            {
                return BadRequest($"No customer with id = {id}. Try again");
            }
            var customerDto = new CustomerDTO()
            {
                ID = customer.CustomerId,
                Name = customer.CustomerName,
                Address = customer.Address,
                Phone = customer.PhoneNumber
            };
            return Ok(customerDto);
        }

        [HttpPost("AddNewCustomer")]
        public IActionResult AddNewCustomer([FromBody]CustomerDTO customer)
        {
            var isAdded = _customerRepo.AddCustomer(customer);
            if (!isAdded) { return BadRequest("Somthing went wrong!"); }

            return Ok(customer);
        }

        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomer([FromBody] CustomerDTO customer)
        {
            if (customer == null)
                return BadRequest("Invalid customer data.");
            try
            {
                var result = _customerRepo.UpdateCustomer(customer);
                if (result == null)
                {
                    return BadRequest($"No Customer with id: {customer.ID}. Try again");
                }

                return Ok(result);
            }
            catch(Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _customerRepo.DeleteCustomer(id);
            if (customer == null)
            {
                return BadRequest($"No customer with id = {id}. Try again");
            }
            var customerdto = new CustomerDTO()
            {
                ID=customer.CustomerId,
                Name=customer.CustomerName,
            };
            return Ok(customerdto);
        }
    }
}
