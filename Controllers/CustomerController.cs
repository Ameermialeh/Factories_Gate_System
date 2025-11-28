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
            return Ok(customers);
        }

        [HttpGet("GetCustomerByID/{id}")]
        public IActionResult GetCustomerByID(int id)
        {
            var customer = _appDbContext.customer.FirstOrDefault(c=>c.CustomerId == id);
            if(customer == null)
            {
                return BadRequest($"No customer with id = {id}. Try again");
            }
            return Ok(customer);
        }

        [HttpPost("AddNewCustomer")]
        public IActionResult AddNewCustomer([FromBody]Customer customer)
        {
            _appDbContext.customer.Add(customer);
            _appDbContext.SaveChanges();
            return Ok();

        }
    }
}
