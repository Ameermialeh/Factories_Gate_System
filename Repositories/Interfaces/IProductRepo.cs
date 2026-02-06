using FactoriesGateSystem.Models.DTOs.ProductDTOs;
using FactoriesGateSystem.Models;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories.Interfaces
{
    public interface IProductRepo
    {
        Task<List<ProductResponseDTO>> GetProductsAsync(string ln, Expression<Func<Product, bool>>? filter = null);
        Task<ProductResponseDTO?> GetProductByIdAsync(int id, string ln);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO productdto, int factoryId);
        Task<ProductDTO?> UpdateProductAsync(int id, UpdateProductDTO dto, int factoryId);
        Task<Product?> DeleteProductAsync(int id, int factoryId);
        Task<bool> ChickIfProductNameExistAsync(string Name, int factoryId);
    }
}
