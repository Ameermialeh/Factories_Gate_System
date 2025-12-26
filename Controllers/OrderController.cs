using FactoriesGateSystem.DTOs.MaterialDTOs;
using FactoriesGateSystem.DTOs.OrderDTOs;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrderRepo _orderRepo;
        public OrderController(OrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orderDto = await _orderRepo.GetOrdersAsync();
                if (orderDto == null) { return NotFound("No order here."); }
                 
                return Ok(orderDto);
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderWithProductsDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrderByID(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid order id.");
            try
            {
                var order = await _orderRepo.GetOrderByIdAsync(id);
                if (order == null) { return NotFound($"No order with id = {id}."); }

                var products = await _orderRepo.GetProductsForOrderAsync(id);

                var dto = new OrderWithProductsDTO()
                {
                    ID = order.OrderId,
                    Name = order.Name,
                    OrderDate = order.OrderDate,
                    CustomerID = order.CustomerId,
                    FactoryId = order.FactoryId,
                    Products = products,
                };

                return Ok(dto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderWithProductsDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOrder([FromBody] OrderWithProductsDTO orderDto)
        {
            try
            {
                var order =await _orderRepo.CreateOrderAsync(orderDto);
                return Ok(order);
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(OrderWithProductsDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderWithProductsDTO orderDto)
        {
            try
            {
                var orderWithProductsDto =await _orderRepo.UpdateOrderAsync(orderDto);
                if (orderWithProductsDto == null) { return NotFound($"No order with id = {orderDto.ID}."); }

                return Ok(orderWithProductsDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid order id.");
            try
            {
                var order = await _orderRepo.DeleteOrderAsync(id);
                if (order == null) { return NotFound($"No order with id = {id}."); }
                var orderdto = new OrderDTO()
                {
                    ID = order.OrderId,
                    Name = order.Name,
                    OrderDate = order.OrderDate,
                    CustomerID = order.CustomerId,
                    FactoryId = order.FactoryId,
                };
                return Ok(orderdto);
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
