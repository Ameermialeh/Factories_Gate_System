using FactoriesGateSystem.Models.DTOs.ProductDTOs;

namespace FactoriesGateSystem.Services.ServiceInterfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetAllProductsAsync(string ln);
        Task<List<ProductResponseDTO>> GetAllProductsWithNameAsync(string ln, string name);
        Task<ProductResponseDTO> GetProductByIdAsync(int id, string ln);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO dto);
        Task<ProductDTO> UpdateProductAsync(int id, UpdateProductDTO dto);
        Task<DeleteProductDTO> DeleteProductAsync(int id);
    }
}
