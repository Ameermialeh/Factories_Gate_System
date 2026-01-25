using FactoriesGateSystem.Models;
using FactoriesGateSystem.Models.DTOs.ProductDTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FactoriesGateSystem.Repositories
{
    public class ProductRepo
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepo(AppDbContext context)
        {
            _appDbContext = context;
        }

        public async Task<List<ProductResponseDTO>> GetProductsAsync(string ln, Expression<Func<Product,bool>>? filter = null)
        {
            IQueryable<Product> query = _appDbContext.products;
            if (filter != null)
                query = query.Where(filter);

            return await query.Select(p => new ProductResponseDTO()
            {
                ID = p.ProductId,
                Name = ln.ToLower() == "ar"? p.NameAr : p.Name ,
                Price = p.Price,
                Quantity = p.Inventory!.Quantity
            }).ToListAsync();
        }

        public async Task<ProductResponseDTO?> GetProductByIdAsync(int id, string ln)
        {
                return await _appDbContext.products.Where(p => p.ProductId == id).Select(p => new ProductResponseDTO {
                    ID = p.ProductId,
                    Name = ln.ToLower() == "ar" ? p.NameAr : p.Name,
                    Price = p.Price,
                    Quantity = p.Inventory!.Quantity 
                }).FirstOrDefaultAsync();
        }

        public async Task<ProductDTO> CreateProductAsync(ProductDTO productdto, int factoryId)
        {
            InventoryProduct inventory = new InventoryProduct
            {
                Quantity = productdto.Quantity,
                LastUpdated = DateTime.UtcNow.Date,
            };
            await _appDbContext.inventoryProducts.AddAsync(inventory);
            await _appDbContext.SaveChangesAsync();

            Product product = new Product
            {
                Name = productdto.Name!,
                NameAr = productdto.NameAr!,
                Price = productdto.Price,
                FactoryId = factoryId,
                InventoryId = inventory.InventoryId,
            };
            await _appDbContext.products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();

            return new ProductDTO
            {
                ID = product.ProductId,
                Name = product.Name,
                NameAr = product.NameAr,
                Price = product.Price,
                Quantity = inventory.Quantity
            };
        }

        public async Task<ProductDTO?> UpdateProductAsync(int id, UpdateProductDTO dto, int factoryId)
        {
            var product = await _appDbContext.products.FirstOrDefaultAsync(p => p.ProductId == id && p.FactoryId == factoryId);
            if (product == null) { return null; }

            if (dto.Name != null)
                product.Name = dto.Name;

            if (dto.Price != null)
                product.Price = dto.Price.Value;

            var inventory = await _appDbContext.inventoryProducts.FirstOrDefaultAsync(i => i.InventoryId == product.InventoryId);

            if (inventory != null && dto.Quantity != null)
            {
                inventory.Quantity = dto.Quantity.Value;
                inventory.LastUpdated = DateTime.UtcNow;
            }

            await _appDbContext.SaveChangesAsync();

            return new ProductDTO
            {
                ID = product.ProductId,
                Name = product.Name,
                NameAr = product.NameAr,
                Price = product.Price,
                Quantity = inventory?.Quantity ?? 0
            };
        }
     
        public async Task<Product?> DeleteProductAsync(int id, int factoryId)
        {
            var product = await _appDbContext.products.FirstOrDefaultAsync(p => p.ProductId == id && p.FactoryId == factoryId);
            if(product == null) return null; 

            var inventory = await _appDbContext.inventoryProducts.FirstOrDefaultAsync(i => i.InventoryId == product.InventoryId);

            _appDbContext.inventoryProducts.Remove(inventory!);
            _appDbContext.products.Remove(product);

            await _appDbContext.SaveChangesAsync();

            return product;
        }

        public async Task<bool> ChickIfProductNameExistAsync(string Name, int factoryId)
        {
            return await _appDbContext.products.AnyAsync(m => m.Name == Name && m.FactoryId == factoryId);
        }
    }
}
