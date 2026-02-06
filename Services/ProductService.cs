using FactoriesGateSystem.Models.DTOs.ProductDTOs;
using FactoriesGateSystem.Repositories.Interfaces;
using FactoriesGateSystem.Services.ServiceInterfaces;

namespace FactoriesGateSystem.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;
        private readonly ICookieService _cookieService;
        public ProductService(IProductRepo productRepo, ICookieService cookieService)
        {
            _productRepo = productRepo;
            _cookieService = cookieService;
        }

        public async Task<List<ProductResponseDTO>> GetAllProductsAsync(string ln)
        {
            var product = await _productRepo.GetProductsAsync(ln);
            return product;
        }
        public async Task<List<ProductResponseDTO>> GetAllProductsWithNameAsync(string ln, string name)
        {
            var products = await _productRepo.GetProductsAsync(ln, p => p.Name.Contains(name));
            return products;
        }
        public async Task<ProductResponseDTO> GetProductByIdAsync(int id, string ln)
        {
            var product = await _productRepo.GetProductByIdAsync(id, ln)
                ?? throw new BadHttpRequestException("Product not found", StatusCodes.Status404NotFound);
            return product;
        }
        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto)
        {
            var factoryId = _cookieService.Get("FactoryId")
               ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var product = await _productRepo.CreateProductAsync(dto, int.Parse(factoryId));
            return product;
        }
        public async Task<ProductDTO> UpdateProductAsync(int id, UpdateProductDTO dto)
        {
            var factoryId = _cookieService.Get("FactoryId")
              ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            if (dto.Name != null)
            {
                var nameExists = await _productRepo.ChickIfProductNameExistAsync(dto.Name, int.Parse(factoryId));

                if (!nameExists)
                    throw new BadHttpRequestException("Product name already exists.", StatusCodes.Status409Conflict);
            }

            var Product = await _productRepo.UpdateProductAsync(id, dto, int.Parse(factoryId))
                ?? throw new BadHttpRequestException("Product not found", StatusCodes.Status404NotFound);

            return Product;
        }
        public async Task<DeleteProductDTO> DeleteProductAsync(int id)
        {
            var factoryId = _cookieService.Get("FactoryId")
             ?? throw new BadHttpRequestException("Unauthorized User", StatusCodes.Status401Unauthorized);

            var product = await _productRepo.DeleteProductAsync(id, int.Parse(factoryId))
            ?? throw new BadHttpRequestException("Product not found", StatusCodes.Status404NotFound);

            var productDto = new DeleteProductDTO()
            {
                ID = id,
                Name = product.Name,
            };
            return productDto;
        }
    }
}
