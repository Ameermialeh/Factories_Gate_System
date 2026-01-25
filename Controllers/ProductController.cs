using FactoriesGateSystem.Models.DTOs.ProductDTOs;
using FactoriesGateSystem.Repositories;
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
        public ProductController(ProductRepo productRepo)
        {
            _productRepo = productRepo;
        }


        [HttpGet]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProducts([FromQuery] string? name, [FromQuery] string ln = "en")
        {
            try
            {
                if (name == null)
                {
                    var productDto = await _productRepo.GetProductsAsync(ln);
                    return Ok(productDto);
                }
                var filtered = await _productRepo.GetProductsAsync(ln, p => p.Name.Contains(name));
                return Ok(filtered);
            }
            catch (Exception) { return StatusCode(500, "Internal server error"); }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProductById(int id, [FromQuery] string ln = "en") {
            if (id <= 0)
                return BadRequest("Invalid product id.");
            try
            {
                var product = await _productRepo.GetProductByIdAsync(id, ln);
                if (product == null)
                    return NotFound($"No product with id = {id}. Try again");
                return Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDto)
        {
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var product = await _productRepo.CreateProductAsync(productDto, int.Parse(factoryId));
                return Ok(product);
            }catch (Exception) { return StatusCode(500, "Internal server error"); }
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO productDto)
        {
            if (id <= 0)
                return BadRequest("Invalid product id.");

            if (productDto.Name == null && productDto.Quantity == null && productDto.Price == null)
                return BadRequest("At least one field (name or quantity or price) must be provided.");

            if (productDto.Quantity < 0 || productDto.Price < 0)
                return BadRequest("Quantity and Price cannot be negative.");
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                if (productDto.Name != null)
                {
                    var nameExists = await _productRepo.ChickIfProductNameExistAsync(productDto.Name, int.Parse(factoryId));

                    if (!nameExists)
                        return NotFound("Product name already exists.");
                }
                var Product = await _productRepo.UpdateProductAsync(id, productDto, int.Parse(factoryId));
                if(Product == null)
                    return NotFound($"No Product with id: {id}. Try again");
               
                return Ok(Product);

            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
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
            try
            {
                var factoryId = Request.Cookies["FactoryId"];
                if (factoryId == null)
                    return Unauthorized();

                var product = await _productRepo.DeleteProductAsync(id,int.Parse(factoryId));
                if (product == null)
                    return NotFound($"No Product with id: {id}. Try again");

                var productDto = new DeleteProductDTO()
                {
                    ID = id,
                    Name = product.Name,
                };
                return Ok(productDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
           
        }
    }
}
