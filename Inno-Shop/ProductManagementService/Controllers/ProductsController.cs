using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagementService.DTOs;
using ProductManagementService.Models;
using ProductManagementService.Services;

namespace ProductManagementService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User, Admin")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetProductsAsync(User);
                return !products.Any() ? NotFound("Could not find any product.") : Ok(products);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id, User);
                return product is null ? NotFound("Such product was not found.") : Ok(product);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductDTO productDTO)
        {
            try
            {
                var product = await _productService.CreateProductAsync(productDTO, User);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDTO)
        {
            try
            {
                await _productService.UpdateProductAsync(id, productDTO, User);
                return Ok("Successfully updated product.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id, User);
                return Ok("Successfully deleted product.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}