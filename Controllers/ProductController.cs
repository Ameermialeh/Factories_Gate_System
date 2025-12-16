using FactoriesGateSystem.DTOs;
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
            var products = _productRepo.GetProducts();
            if (products == null)
            {
                return BadRequest("No Products here!!");
            }
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

        [HttpGet("GetProductById/{id}")]
        public IActionResult GetProductById(int id) {
            var product = _productRepo.GetProductById(id);
            if (product == null)
            {
                return BadRequest($"No product with id = {id}. Try again");
            }
            var productdto = new ProductDTO()
            {
                ID = product.ProductId,
                Name = product.ProductName,
                Description = product.ProductDescription,
                Price = product.ProductPrice,
                Quantity= product.ProductQuantity,
            };
            return Ok(productdto);
        }

        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct([FromBody] ProductDTO productdto)
        {
            var productResult = _productRepo.CreateProduct(productdto);
            if (productResult == null) { return BadRequest("Somthing went wrong!"); }
            return Ok(productResult);
        }

        [HttpPut("UpdateProduct")]
        public IActionResult UpdateProduct([FromBody] ProductDTO productdto)
        {
            if (productdto == null)
                return BadRequest("Invalid product data.");
            try
            {
               var updatedProduct = _productRepo.UpdateProduct(productdto);
                if(updatedProduct == null)
                {
                    return BadRequest($"No Product with id: {productdto.ID}. Try again");
                }
                return Ok(updatedProduct);

            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteProduct/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _productRepo.DeleteProduct(id);
            if(product == null)
            {
                return BadRequest($"No Product with id: {id}. Try again");
            }
            var productdto = new ProductDTO()
            {
                ID = id,
                Name = product.ProductName,
                Description = product.ProductDescription,
                Price = product.ProductPrice,
                Quantity = product.ProductQuantity,
                
            };
            return Ok(productdto);
        }
    }
}
