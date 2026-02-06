using FactoriesGateSystem.Models.DTOs.OrderDTOs;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService )
        {
            _orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrderDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrders([FromQuery] string? name)
        {
 
            if (name == null)
            {
                var order = await _orderService.GetAllOrdersAsync();
                return Ok(order);
            }
            var filtered = await _orderService.GetAllOrdersWithName(name);
            return Ok(filtered);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(OrderResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrderByID(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid order id.");

            var order = await _orderService.GetOrderByIDAsync(id);
            return Ok(order);
        }


        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<OrderResponseDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrdersByName(string name)
        {
            var orders = await _orderService.GetOrdersByNameAsync(name);
            return Ok(orders);
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOrder( [FromBody] OrderWithProductsDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(dto);
            return Ok(order); 
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OrderWithProductsDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO dto)
        {
            if(id <= 0)
                return BadRequest("Invalid order id.");

            if(dto.Name == null && dto.OrderDate == null && dto.CustomerID == null && dto.Products == null)
                return BadRequest("At least one field (Name or OrderDate or CustomerID or Products) must be provided.");

            if(dto.CustomerID <=0)
                return BadRequest("Invalid customer id.");
            
            var order = await _orderService.UpdateOrderAsync(id, dto);
            return Ok(order);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid order id.");

            await _orderService.DeleteOrderAsync(id);
            return Ok("Deleted Order Successfully");
        }

    }
}
