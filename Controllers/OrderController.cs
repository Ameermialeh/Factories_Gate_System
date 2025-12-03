using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class OrderController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly OrderRepo _orderRepo;
        public OrderController(AppDbContext appDbContext, OrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
            _appDbContext = appDbContext;
        }

        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            var orders = _orderRepo.GetOrders();
            if(orders == null)
            {
                return BadRequest("No order here!!");
            }
            return Ok(orders);
        }

        [HttpGet("GetOrderByID/{id}")]
        public IActionResult GetOrderByID(int id)
        {
            var order = _orderRepo.GetOrderById(id);
            if(order == null)
            {
                return BadRequest($"No order with id = {id}. Try again");
            }
            return Ok(order);
        }

        [HttpPost("AddNewOrder")]
        public IActionResult AddNewOrder([FromBody] Order order)
        {
           var isAdded = _orderRepo.AddOrder(order);
            if (!isAdded) { return BadRequest("Somthing went wrong!"); }
            return Ok(order);
        }
    }
}
