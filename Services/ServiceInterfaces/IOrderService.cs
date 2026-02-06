using FactoriesGateSystem.Models.DTOs.OrderDTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<List<OrderDTO>> GetAllOrdersWithName(string name);
        Task<OrderResponseDTO> GetOrderByIDAsync(int id);
        Task<List<OrderResponseDTO>> GetOrdersByNameAsync(string name);
        Task<OrderResponseDTO> CreateOrderAsync(OrderWithProductsDTO dto);
        Task<OrderWithProductsDTO> UpdateOrderAsync(int id, UpdateOrderDTO dto);
        Task DeleteOrderAsync(int id);
    }
}
