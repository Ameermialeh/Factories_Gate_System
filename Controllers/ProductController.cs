using FactoriesGateSystem.Models.DTOs.ProductDTOs;
using FactoriesGateSystem.Repositories;
using FactoriesGateSystem.Services.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "manager")]
    public class ProductController : Controller
    {
        private readonly ProductRepo _productRepo;
        private readonly IProductService _productService;
        public ProductController(ProductRepo productRepo, IProductService productService)
        {
            _productRepo = productRepo;
            _productService = productService;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<ProductResponseDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProducts([FromQuery] string? name, [FromQuery] string ln = "en")
        {
            if (name == null)
            {
                var product = await _productService.GetAllProductsAsync(ln);
                return Ok(product);
            }
            var filtered = await _productService.GetAllProductsWithNameAsync(ln, name);
            return Ok(filtered);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProductById(int id, [FromQuery] string ln = "en") {
            if (id <= 0)
                return BadRequest("Invalid product id.");

            var product = await _productService.GetProductByIdAsync(id, ln);
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = await _productService.CreateProductAsync(dto);
            return Ok(product);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO dto)
        {
            if (id <= 0)
                return BadRequest("Invalid product id.");

            if (dto.Name == null && dto.Quantity == null && dto.Price == null)
                return BadRequest("At least one field (name or quantity or price) must be provided.");

            if (dto.Quantity < 0 || dto.Price < 0)
                return BadRequest("Quantity and Price cannot be negative.");

            var product = await _productService.UpdateProductAsync(id, dto);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DeleteProductDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid product id.");

            var product = await _productService.DeleteProductAsync(id);
            return Ok(product);
        }
    }
}
