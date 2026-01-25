using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.OrderDTOs;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class OrderController : Controller
    {
        private readonly OrderRepo _orderRepo;
        public OrderController(OrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<OrderDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrders([FromQuery] string? name)
        {
            try
            {
                if (name == null)
                {
                    var orderDto = await _orderRepo.GetOrdersAsync();
                    return Ok(orderDto);
                }
                var filtered = await _orderRepo.GetOrdersAsync(o => o.Name.Contains(name));
                return Ok(filtered);
            }
            catch (Exception) { return StatusCode(500, "Internal server error"); }
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
            try
            {
                var order = await _orderRepo.GetOrderByIdAsync(id);
                if (order == null) { return NotFound($"No order with id = {id}."); }

                var products = await _orderRepo.GetProductsForOrderAsync(id);

                var dto = new OrderResponseDTO()
                {
                    ID = order.OrderId,
                    Name = order.Name,
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


        [HttpGet("{name:alpha}")]
        [ProducesResponseType(typeof(List<OrderResponseDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetOrdersByName(string name)
        {
            try
            {
                var orders = await _orderRepo.GetOrdersAsync(o => o.Name.Contains(name));

                var result = new List<OrderResponseDTO>();

                foreach (var order in orders)
                {
                    var products = await _orderRepo.GetProductsForOrderAsync(order.ID);

                    result.Add(new OrderResponseDTO
                    {
                        ID = order.ID,
                        Name = order.Name,
                        OrderDate = order.OrderDate,
                        CustomerID = order.CustomerID,
                        Products = products
                    });
                }

                return Ok(result);
            }
            catch (Exception) { return StatusCode(500, "Internal server error"); }
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateOrder( [FromBody] OrderWithProductsDTO dto)
        {
            if (dto.Name == null && dto.OrderDate == null && dto.CustomerID == null && dto.Products == null)
                return BadRequest("All fields (Name or OrderDate or CustomerID or Products) must be provided.");
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var order =await _orderRepo.CreateOrderAsync(dto, int.Parse(factoryId));
                if (order == null) { return BadRequest(""); }
                return Ok(order);
            }
            catch (Exception) { return StatusCode(500, "Internal server error"); }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(OrderWithProductsDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderWithProductsDTO dto)
        {
            if(id <= 0)
                return BadRequest("Invalid order id.");
            if(dto.Name == null && dto.OrderDate == null && dto.CustomerID == null && dto.Products == null)
                return BadRequest("At least one field (Name or OrderDate or CustomerID or Products) must be provided.");
            if(dto.CustomerID <=0)
                return BadRequest("Invalid customer id.");
            try
            {
                var orderWithProductsDto =await _orderRepo.UpdateOrderAsync(id, dto);
                if (orderWithProductsDto == null) { return NotFound($"No order with id = {id}."); }

                return Ok(orderWithProductsDto);
            }
            catch (Exception) { return StatusCode(500, "Internal server error"); }
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
            try
            {
                var done = await _orderRepo.DeleteOrderAsync(id);
                if (!done) { return NotFound($"No order with id = {id}."); }
                
                return Ok("Deleted Order Successfully");
            }
            catch (Exception) {
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
