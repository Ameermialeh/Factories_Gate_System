using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly CustomerRepo _customerRepo;
        public CustomerController(AppDbContext appDbContext, CustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
            _appDbContext = appDbContext;
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
                Name = customer.CustomerName,
                Address = customer.Address,
                Phone = customer.PhoneNumber
            };
            return Ok(customerDto);
        }

        [HttpPost("AddNewCustomer")]
        public IActionResult AddNewCustomer([FromBody]Customer customer)
        {
            var isAdded = _customerRepo.AddCustomer(customer);
            if (!isAdded) { return BadRequest("Somthing went wrong!"); }

            var customerDto = new CustomerDTO()
            {
                Name = customer.CustomerName,
                Address = customer.Address,
                Phone = customer.PhoneNumber
            };
            return Ok(customerDto);
        }

        [HttpPut("UpdateCustomer")]
        public IActionResult UpdateCustomer([FromBody] Customer customer)
        {
            var newCustomer = _customerRepo.UpdateCustomer(customer);
            if (newCustomer == null)
            {
                return BadRequest($"Somthing went wrong. Try again");
            }
            return Ok(newCustomer);
        }

        [HttpDelete("DeleteCustomer/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _customerRepo.DeleteCustomer(id);
            if (customer == null)
            {
                return BadRequest($"No customer with id = {id}. Try again");
            }
            return Ok(customer);
        }
    }
}
