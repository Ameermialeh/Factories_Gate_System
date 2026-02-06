using FactoriesGateSystem.Models.DTOs.OrderDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IOrderRepo
    {
        Task<List<OrderDTO>> GetOrdersAsync(Expression<Func<Order, bool>>? filter = null);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<List<OrderItemDTO>> GetProductsForOrderAsync(int orderID);
        Task<OrderResponseDTO?> CreateOrderAsync(OrderWithProductsDTO dto, int factoryId);
        Task<OrderWithProductsDTO?> UpdateOrderAsync(int id, UpdateOrderDTO dto);
        Task<bool> DeleteOrderAsync(int id);
    }
}
