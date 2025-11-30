using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CustomerController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            var customers = _appDbContext.customer.ToList();
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
            var customer = _appDbContext.customer.FirstOrDefault(c=>c.CustomerId == id);
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
            _appDbContext.customer.Add(customer);
            _appDbContext.SaveChanges();

            var customerDto = new CustomerDTO()
            {
                Name = customer.CustomerName,
                Address = customer.Address,
                Phone = customer.PhoneNumber
            };
            return Ok(customerDto);
        }

        [HttpPut("UpdateCustomer/{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer customer)
        {
            var newCustomer = _appDbContext.customer.FirstOrDefault(c=>c.CustomerId == id );
            if (newCustomer == null)
            {
                return BadRequest($"No customer with id = {id}. Try again");
            }
            newCustomer.PhoneNumber = customer.PhoneNumber;
            newCustomer.Address = customer.Address;
            newCustomer.CustomerName = customer.CustomerName;
            newCustomer.CurrentBalance = customer.CurrentBalance;
            _appDbContext.SaveChanges();
            return Ok(newCustomer);

        }

        [HttpDelete("DeleteCustomer/{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _appDbContext.customer.FirstOrDefault(c => c.CustomerId == id);
            if (customer == null)
            {
                return BadRequest($"No customer with id = {id}. Try again");
            }
            var orders = customer.Orders;
            if(orders != null )
            {
                foreach( var order in orders )
                {
                    _appDbContext.orders.Remove(order);

                }
            }
            _appDbContext.customer.Remove(customer);
            _appDbContext.SaveChanges();
            return Ok(customer);
        }
    }
}
