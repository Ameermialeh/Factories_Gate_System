using FactoriesGateSystem.DTOs.OrderDTOs;
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
            try
            {
                var orders = _orderRepo.GetOrders();
                if (orders == null) { return NotFound("No order here."); }
                var orderDto = orders.Select(o => new OrderDTO()
                {
                    ID = o.OrderId,
                    Name = o.OrderName,
                    Description = o.OrderDescription,
                    OrderDate = o.OrderDate,
                    CustomerID = o.CustomerId
                });
                return Ok(orderDto);
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetOrderByID/{id}")]
        public IActionResult GetOrderByID(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid order id.");
            try
            {
                var order = _orderRepo.GetOrderById(id);
                if (order == null) { return NotFound($"No order with id = {id}."); }

                var products = _orderRepo.GetProductsForOrder(id);

                var dto = new OrderWithProductsDTO()
                {
                    ID = order.OrderId,
                    Name = order.OrderName,
                    Description = order.OrderDescription,
                    OrderDate = order.OrderDate,
                    CustomerID = order.CustomerId,
                    Products = products,
                };

                return Ok(dto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateOrder")]
        public IActionResult CreateOrder([FromBody] OrderWithProductsDTO orderDto)
        {
            try
            {
                var productAvailable = _orderRepo.ChickIfAllProductsNotHide(orderDto);
                if (!productAvailable) { return BadRequest("Product not found!"); }

                var order = _orderRepo.CreateOrder(orderDto);
                return Ok(order);
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateOrder")] 
        public IActionResult UpdateOrder([FromBody] OrderWithProductsDTO orderDto)
        {
            try
            {
                var orderWithProductsDto = _orderRepo.UpdateOrder(orderDto);
                if (orderWithProductsDto == null) { return NotFound($"No order with id = {orderDto.ID}."); }

                return Ok(orderWithProductsDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteOrder/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid order id.");
            try
            {
                var order = _orderRepo.DeleteOrder(id);
                if (order == null) { return BadRequest($"No order with id = {id}."); }
                var orderdto = new OrderDTO()
                {
                    ID = order.OrderId,
                    Name = order.OrderName,
                    OrderDate = order.OrderDate,
                    CustomerID = order.CustomerId
                };
                return Ok(orderdto);
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
