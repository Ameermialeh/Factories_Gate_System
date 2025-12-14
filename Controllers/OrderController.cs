using FactoriesGateSystem.DTOs;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrderRepo _orderRepo;
        public OrderController(OrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpGet("GetAllOrders")]
        public IActionResult GetAllOrders()
        {
            var orders = _orderRepo.GetOrders();
            if(orders == null)
            {
                return BadRequest("No order here!!");
            }
            var corderDto = orders.Select(o => new OrderDTO()
            {
                ID = o.OrderId,
                Name = o.OrderName,
                Description = o.OrderDescription,
                OrderDate = o.OrderDate,
                CustomerID = o.CustomerId
            });
            return Ok(corderDto);
        }

        [HttpGet("GetOrderByID/{id}")]
        public IActionResult GetOrderByID(int id)
        {
            var order = _orderRepo.GetOrderById(id);
            if(order == null)
            {
                return BadRequest($"No order with id = {id}. Try again");
            }
            var products = _orderRepo.GetProductsForOrder(id);

            var OrderWithProductsDto = new OrderWithProductsDTO()
            {
                ID = order.OrderId,
                Name = order.OrderName,
                Description = order.OrderDescription,
                OrderDate = order.OrderDate,
                CustomerID = order.CustomerId,
                Products = products,
            };

            return Ok(OrderWithProductsDto);
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] OrderWithProductsDTO order)
        {
           var isAdded = _orderRepo.CreateOrder(order);
            if (!isAdded) { return BadRequest("Somthing went wrong!"); }
            return Ok(order);
        }

        [HttpPut("UpdateOrder")] // Here
        public IActionResult UpdateOrder([FromBody] OrderDTO orderdto)
        {
            var newOrder = _orderRepo.UpdateOrder(orderdto);
            if (newOrder == null)
            {
                return BadRequest($"Somthing went wrong. Try again");
            }
            return Ok(newOrder);
        }

        [HttpDelete("DeleteOrder/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _orderRepo.DeleteOrder(id);
            if (order == null)
            {
                return BadRequest($"No customer with id = {id}. Try again");
            }
            var orderdto = new OrderDTO()
            {
                ID = order.OrderId,
                Name = order.OrderName,
                OrderDate = order.OrderDate,
                CustomerID = order.CustomerId
            };
            return Ok(orderdto);
        }
    }
}
