using FactoriesGateSystem.DTOs.MaterialDTOs;
using FactoriesGateSystem.DTOs.ProductDTOs;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var productDto = await _productRepo.GetProductsAsync();
                if (productDto == null || productDto.Count == 0) { return NotFound("There is no products."); }

                return Ok(productDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProductById(int id) {
            if (id <= 0)
                return BadRequest("Invalid product id.");
            try
            {
                var product = await _productRepo.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound($"No product with id = {id}. Try again");

                var productDto = new ProductDTO()
                {
                    ID = product.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.StockQuantity,
                    FactoryId = product.FactoryId,
                };
                return Ok(productDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDto)
        {
            try
            {
                var product =await _productRepo.CreateProductAsync(productDto);
                if (product == null) { return BadRequest("Somthing went wrong!"); }
                return Ok(product);

            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
           
        }

        [HttpPut]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO productDto)
        {
            if (productDto == null)
                return BadRequest("Invalid product data.");
            try
            {
               var updatedProduct = await _productRepo.UpdateProductAsync(productDto);
                if(updatedProduct == null)
                    return NotFound($"No Product with id: {productDto.ID}. Try again");
               
                return Ok(updatedProduct);

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
                var product = await _productRepo.DeleteProductAsync(id);
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
