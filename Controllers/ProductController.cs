using FactoriesGateSystem.DTOs.ProductDTOs;
using FactoriesGateSystem.Models;
using FactoriesGateSystem.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FactoriesGateSystem.Controllers
{
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductRepo _productRepo;
        public ProductController(ProductRepo productRepo)
        {
            _productRepo = productRepo;
        }


        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            try
            {
                var products = _productRepo.GetProducts();

                if (products == null || products.Count == 0) { return BadRequest("There is no products."); }

                var productDto = products.Select(p => new ProductDTO()
                {
                    ID = p.ProductId,
                    Name = p.ProductName,
                    Description = p.ProductDescription,
                    Price = p.ProductPrice,
                    Quantity = p.ProductQuantity,
                });
                return Ok(productDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById(int id) {
            if (id <= 0)
                return BadRequest("Invalid product id.");
            try
            {
                var product = _productRepo.GetProductById(id);
                if (product == null)
                    return NotFound($"No product with id = {id}. Try again");

                var productDto = new ProductDTO()
                {
                    ID = product.ProductId,
                    Name = product.ProductName,
                    Description = product.ProductDescription,
                    Price = product.ProductPrice,
                    Quantity = product.ProductQuantity,
                };
                return Ok(productDto);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct([FromBody] ProductDTO productDto)
        {
            try
            {
                var product = _productRepo.CreateProduct(productDto);
                if (product == null) { return BadRequest("Somthing went wrong!"); }
                return Ok(product);

            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
           
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct([FromBody] ProductDTO productDto)
        {
            if (productDto == null)
                return BadRequest("Invalid product data.");
            try
            {
               var updatedProduct = _productRepo.UpdateProduct(productDto);
                if(updatedProduct == null)
                    return NotFound($"No Product with id: {productDto.ID}. Try again");
               
                return Ok(updatedProduct);

            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid product id.");
            try
            {
                var product = _productRepo.DeleteProduct(id);
                if (product == null)
                    return BadRequest($"No Product with id: {id}. Try again");

                var productDto = new DeleteProductDTO()
                {
                    ID = id,
                    Name = product.ProductName,
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
