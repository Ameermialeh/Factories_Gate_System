using FactoriesGateSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class OrderController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public OrderController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            var orders = _appDbContext.orders.ToList();
            if(orders == null)
            {
                return BadRequest("No order here!!");
            }
            return Ok(orders);
        }

        [HttpGet("GetOrderByID/{id}")]
        public IActionResult GetOrderByID(int id)
        {
            var order = _appDbContext.orders.FirstOrDefault(o => o.OrderId == id);
            if(order == null)
            {
                return BadRequest($"No order with id = {id}. Try again");
            }
            return Ok(order);
        }

        [HttpPost("AddNewOrder")]
        public IActionResult AddNewOrder([FromBody] Order Order)
        {
            _appDbContext.orders.Add(Order);
            _appDbContext.SaveChanges();
            return Ok();
        }
    }
}
