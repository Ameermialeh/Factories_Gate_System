using FactoriesGateSystem.Models.DTOs.OrderDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICookieService _cookieService;
        public OrderService(IOrderRepo orderRepo, ICookieService cookieService)
        {
            _orderRepo = orderRepo;
            _cookieService = cookieService;
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var order = await _orderRepo.GetOrdersAsync();
            return order;
        }
        public async Task<List<OrderDTO>> GetAllOrdersWithName(string name)
        {
            var filtered = await _orderRepo.GetOrdersAsync(o => o.Name.Contains(name));
            return filtered;
        }
        public async Task<OrderResponseDTO> GetOrderByIDAsync(int id)
        {
            var order = await _orderRepo.GetOrderByIdAsync(id);
            if (order == null) { throw new BadHttpRequestException("Order not found", StatusCodes.Status404NotFound); }

            var products = await _orderRepo.GetProductsForOrderAsync(id);

            var dto = new OrderResponseDTO()
            {
                ID = order.OrderId,
                Name = order.Name,
                OrderDate = order.OrderDate,
                CustomerID = order.CustomerId,
                Products = products,
            };
            return dto;
        }
        public async Task<List<OrderResponseDTO>> GetOrdersByNameAsync(string name)
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

            return result;
        }
        public async Task<OrderResponseDTO> CreateOrderAsync(OrderWithProductsDTO dto)
        {
            var factoryId = _cookieService.Get("FactoryId")
               ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var order = await _orderRepo.CreateOrderAsync(dto, int.Parse(factoryId));
            if (order == null) { throw new BadHttpRequestException("Product not found", StatusCodes.Status400BadRequest); }

            return order;
        }

        public async Task<OrderWithProductsDTO> UpdateOrderAsync(int id, UpdateOrderDTO dto)
        {
            var order = await _orderRepo.UpdateOrderAsync(id, dto)
                ?? throw new BadHttpRequestException("Order not found", StatusCodes.Status404NotFound); 
            return order;
        }
        public async Task DeleteOrderAsync(int id)
        {
            var done = await _orderRepo.DeleteOrderAsync(id);
            if (!done) { throw new BadHttpRequestException("Order not found", StatusCodes.Status404NotFound); }
        }
    }
}
